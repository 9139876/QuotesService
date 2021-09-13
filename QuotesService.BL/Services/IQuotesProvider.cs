using System.Collections.Generic;
using System.Threading.Tasks;
using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;

namespace QuotesService.BL.Services
{
    public interface IQuotesProvider
    {
        Task<GetQuotesResponse> GetLastBatchQuotes(TickerMarketTimeFrame request);

        Task<StandartResponse> CheckGetQuotes(CheckGetQuotesRequest request);

        Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request);

        Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request);

        List<TimeFrameEnum> GetAvailableTimeFrames();
    }
}
