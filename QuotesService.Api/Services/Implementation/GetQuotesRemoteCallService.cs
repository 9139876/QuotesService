using CommonLibraries.Graal.Models;
using CommonLibraries.RemoteCall;
using CommonLibraries.RemoteCall.Services;
using Microsoft.Extensions.Configuration;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesService.Api.Services.Implementation
{
    internal class GetQuotesRemoteCallService : BaseRemoteCallService, IGetQuotesRemoteCallService
    {
        public GetQuotesRemoteCallService(
            IConfiguration configuration,
            IRemoteCallHelperService remoteCallHelperService) : base(configuration, remoteCallHelperService) { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "QuotesService.Api.SchemeAndHost";

        public async Task<List<TickerAndMarket>> GetAllTickersAndMarkets(GetAllTickersAndMarketsRequest request)
            => await ExecutePostAsync<List<TickerAndMarket>, GetAllTickersAndMarketsRequest>("api/get-all-tickers-and-markets", request);

        public async Task<GetNearestQuoteResponse> GetNearestQuote(GetNearestQuoteRequest request)
            => await ExecutePostAsync<GetNearestQuoteResponse, GetNearestQuoteRequest>("api/get-nearest-quote", request);

        public async Task<List<QuoteModel>> GetQuotes(GetQuotesRequest request)
            => await ExecutePostAsync<List<QuoteModel>, GetQuotesRequest>("api/get-quotes", request);

        public async Task<QuotesInfo> GetQuotesInfo(TickerMarketTimeFrame request)
            => await ExecutePostAsync<QuotesInfo, TickerMarketTimeFrame>("api/get-quotes-info", request);
    }
}
