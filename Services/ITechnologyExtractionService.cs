using TechStax.Models;

namespace TechStax.Services
{
    public interface ITechnologyExtractionService
    {
        /// <summary>
        /// Count occurrences of each known technology keyword across all supplied texts.
        /// </summary>
        Task<IReadOnlyList<TechnologyStat>> CountTechnologiesAsync(
            IEnumerable<string> texts,
            CancellationToken ct);
    }
}
