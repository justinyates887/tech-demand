using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using TechStax.Models;
using TechStax.Extensions;

namespace TechStax.Services
{
    public interface IJobDataService
    {
        Task RefreshAsync(CancellationToken ct);
        IReadOnlyList<TechnologyStat> GetTechnologyStats();
        IReadOnlyList<JobPosting> GetPostingsForTechnology(string technologyName);
        IReadOnlyList<JobPosting> GetAllPostings();
    }

    public class JobDataService : IJobDataService
    {
        private readonly IEnumerable<IJobBoardScraper> _scrapers;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "JobDataService_Cache";

        public JobDataService(IEnumerable<IJobBoardScraper> scrapers, IMemoryCache cache)
        {
            _scrapers = scrapers;
            _cache = cache;
        }

        /// <summary>
        /// Refreshes the job data if not cached or expired. Cached data expires after 1 hour.
        /// </summary>
        public async Task RefreshAsync(CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheKey, out JobDataCache cached))
            {
                // cache valid, do nothing
                return;
            }

            // Scrape all postings
            var all = new List<JobPosting>();
            foreach (var scraper in _scrapers)
            {
                var postings = await scraper.FetchJobPostingsAsync(string.Empty, ct);
                all.AddRange(postings);
            }

            // Prepare regex map for whole-word matching
            var techNames = Enum.GetValues<Technology>()
                .Select(t => t.GetDescription())
                .ToList();

            var map = techNames.ToDictionary(
                name => name,
                _ => new List<JobPosting>(),
                StringComparer.OrdinalIgnoreCase
            );

            // Group postings by technology
            foreach (var job in all)
            {
                var hay = job.Title + " " + job.Description;
                foreach (var tech in techNames)
                {
                    var pattern = $"(?<![\\w-]){Regex.Escape(tech)}(?![\\w-])";
                    if (Regex.IsMatch(hay, pattern, RegexOptions.IgnoreCase))
                    {
                        map[tech].Add(job);
                    }
                }
            }

            // Store in cache with 1 hour expiration
            var data = new JobDataCache { AllPostings = all, Map = map };
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            _cache.Set(CacheKey, data, options);
        }

        public IReadOnlyList<JobPosting> GetAllPostings()
        {
            if (_cache.TryGetValue(CacheKey, out JobDataCache cached))
                return cached.AllPostings;
            return Array.Empty<JobPosting>();
        }

        public IReadOnlyList<TechnologyStat> GetTechnologyStats()
        {
            if (_cache.TryGetValue(CacheKey, out JobDataCache cached))
            {
                return cached.Map
                    .Select(kv => new TechnologyStat { Name = kv.Key, Count = kv.Value.Count })
                    .OrderByDescending(ts => ts.Count)
                    .ToList();
            }
            return Array.Empty<TechnologyStat>();
        }

        public IReadOnlyList<JobPosting> GetPostingsForTechnology(string technologyName)
        {
            if (_cache.TryGetValue(CacheKey, out JobDataCache cached) &&
                cached.Map.TryGetValue(technologyName, out var list))
            {
                return list;
            }
            return Array.Empty<JobPosting>();
        }

        /// <summary>
        /// Internal cache model to hold scraped postings and grouping map.
        /// </summary>
        private class JobDataCache
        {
            public List<JobPosting> AllPostings { get; set; }
            public Dictionary<string, List<JobPosting>> Map { get; set; }
        }
    }
}
