using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesService.Api.Services
{
    public interface IGetQuotesRemoteCallService
    {
        Task<QuotesInfoResponse> GetQuotesInfo(TickerMarketTimeFrame request);

        Task<List<QuoteModel>> GetQuotes(GetQuotesRequest request);
    }
}
