using QuotesService.BL.Models;
using System.Threading.Tasks;

namespace QuotesService.BL.Services
{
    public interface IQuotesStorageService
    {
        Task<bool> QuotesToStorage(QuotesToStorageRequest request);

        Task<CompareQuotesResponse> CompareQuotes(CompareQuotesRequest request);
    }
}
