using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechStax.Models;

namespace TechStax.Services
{
    public interface IJobBoardScraper
    {
        Task<IEnumerable<string>> FetchJobDescriptionsAsync(string searchTerm, CancellationToken ct);
        Task<IEnumerable<JobPosting>> FetchJobPostingsAsync(string searchTerm, CancellationToken ct);
    }
}
