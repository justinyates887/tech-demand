using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TechStax.Models;
using TechStax.Services;   // for HtmlUtils

public class RemoteOkScraper : IJobBoardScraper
{
    private readonly HttpClient _http;

    public RemoteOkScraper(HttpClient http) => _http = http;

    public async Task<IEnumerable<JobPosting>> FetchJobPostingsAsync(
        string searchTerm,
        CancellationToken ct)
    {
        // 1) Fetch the raw feed
        using var resp = await _http.GetAsync("https://remoteok.com/api", ct);
        if (!resp.IsSuccessStatusCode)
            return Enumerable.Empty<JobPosting>();

        // 2) Parse JSON and skip the first “meta” element
        using var doc = await JsonDocument.ParseAsync(
            await resp.Content.ReadAsStreamAsync(ct),
            default, ct);
        var jobsJson = doc.RootElement.EnumerateArray().Skip(1);

        var results = new List<JobPosting>();
        foreach (var e in jobsJson)
        {
            // 3) Map fields (use description_text to strip HTML)
            var title = e.GetProperty("position").GetString() ?? "";
            var company = e.GetProperty("company").GetString() ?? "";
            var location = e.GetProperty("location").GetString() ?? "";
            var descText = e.TryGetProperty("description_text", out var dt)
                           ? dt.GetString() ?? ""
                           : HtmlUtils.StripHtml(
                               e.GetProperty("description").GetString() ?? "");
            var salary = e.TryGetProperty("salary", out var sal)
                         ? sal.GetString() : null;
            var payRange = string.IsNullOrWhiteSpace(salary)
                           ? "N/A"
                           : salary;

            // 4) Optional keyword filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var hay = $"{title} {descText}";
                if (!hay.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    continue;
            }

            results.Add(new JobPosting
            {
                Title = title,
                Company = company,
                Location = location,
                Description = descText,
                PayRange = payRange
            });
        }

        return results;
    }

    public Task<IEnumerable<string>> FetchJobDescriptionsAsync(
        string searchTerm,
        CancellationToken ct)
        => FetchJobPostingsAsync(searchTerm, ct)
            .ContinueWith(t =>
                t.Result.Select(p => $"{p.Title}\n{p.Description}"),
            ct);
}
