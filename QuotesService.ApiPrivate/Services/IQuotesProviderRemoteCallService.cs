using System.Collections.Generic;
using System.Threading.Tasks;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.ApiPrivate.Models;
using QuotesService.ApiPrivate.Models.RequestResponse;

namespace QuotesService.ApiPrivate.Services
{
    public interface IQuotesProviderRemoteCallService
    {
        Task<CheckGetQuotesResponse> CheckGetQuotes(CheckGetQuotesRequest request);

        Task<GetQuotesResponse> GetQuotes(GetQuotesRequest request);

        Task<GetQuotesProviderResponse> GetQuotesProvider(TickerAndMarketRequest request);

        Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request);

        Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request);
    }
}
