// Services/MuseScraper.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TechStax.Models;
using TechStax.Services;    // for HtmlUtils

namespace TechStax.Services
{
    // models matching TheMuse JSON
    class MuseResponse
    {
        [JsonPropertyName("page")] public int Page { get; set; }
        [JsonPropertyName("results")] public MuseJob[] Results { get; set; }
    }

    class MuseJob
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("contents")] public string Contents { get; set; }
        [JsonPropertyName("locations")] public Location[] Locations { get; set; }
        [JsonPropertyName("company")] public Company Company { get; set; }
    }

    class Location
    {
        [JsonPropertyName("name")] public string Name { get; set; }
    }

    class Company
    {
        [JsonPropertyName("name")] public string Name { get; set; }
    }

    public class MuseScraper : IJobBoardScraper
    {
        private readonly HttpClient _http;
        private const string CategorySlug = "Science%20and%20Engineering";

        public MuseScraper(HttpClient http) => _http = http;

        public async Task<IEnumerable<JobPosting>> FetchJobPostingsAsync(
            string _unusedSearchTerm,
            CancellationToken ct)
        {
            var all = new List<JobPosting>();
            var page = 1;

            while (!ct.IsCancellationRequested)
            {
                if (page > 99)
                    break;
                // Always fetch the Science and Engineering category
                var url =
                    $"https://www.themuse.com/api/public/jobs" +
                    $"?page={page++}" +
                    $"&category={CategorySlug}";

                var resp = await _http.GetAsync(url, ct);
                if (!resp.IsSuccessStatusCode) break;

                var muse = await resp.Content
                                     .ReadFromJsonAsync<MuseResponse>(cancellationToken: ct);
                if (muse?.Results == null || muse.Results.Length == 0)
                    break;

                foreach (var m in muse.Results)
                {
                    var title = m.Name ?? "";
                    var company = m.Company?.Name ?? "N/A";
                    var locs = m.Locations?
                                      .Select(x => x.Name)
                                      .Where(x => !string.IsNullOrWhiteSpace(x));
                    var location = locs != null
                                      ? string.Join(", ", locs)
                                      : "N/A";
                    var desc = HtmlUtils.StripHtml(m.Contents ?? "");

                    all.Add(new JobPosting
                    {
                        Title = title,
                        Company = company,
                        Location = location,
                        Description = desc,
                        PayRange = "N/A"
                    });
                }
            }

            return all;
        }

        public async Task<IEnumerable<string>> FetchJobDescriptionsAsync(
            string searchTerm,
            CancellationToken ct)
        {
            // descriptions only from Science & Engineering postings
            var jobs = await FetchJobPostingsAsync(searchTerm, ct);
            return jobs.Select(p => $"{p.Title}\n{p.Description}");
        }
    }
}
