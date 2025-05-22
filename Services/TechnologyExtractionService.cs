using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuzzySharp;
using TechStax.Models;
using TechStax.Extensions;

namespace TechStax.Services
{
    public class TechnologyExtractionService : ITechnologyExtractionService
    {
        private readonly IList<string> _keywords;
        private const int MatchThreshold = 60;

        public TechnologyExtractionService()
        {
            _keywords = Enum
                .GetValues<Technology>()
                .Select(t => t.GetDescription())
                .ToList();
        }

        public Task<IReadOnlyList<TechnologyStat>> CountTechnologiesAsync(
            IEnumerable<string> texts,
            CancellationToken ct)
        {
            var counts = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(texts, new ParallelOptions { CancellationToken = ct }, text =>
            {
                foreach (var kw in _keywords)
                {
                    // use partial‐ratio so e.g. "Kubernetess" still matches "Kubernetes"
                    var score = Fuzz.PartialRatio(text, kw);
                    if (score >= MatchThreshold)
                    {
                        var key = kw.ToLowerInvariant();
                        counts.AddOrUpdate(key, 1, (_, old) => old + 1);
                    }
                }
            });

            var results = counts
                .Select(kv => new TechnologyStat
                {
                    Name = CultureInfo.CurrentCulture.TextInfo
                               .ToTitleCase(kv.Key),
                    Count = kv.Value
                })
                .OrderByDescending(ts => ts.Count)
                .ToList();

            return Task.FromResult((IReadOnlyList<TechnologyStat>)results);
        }
    }
}
