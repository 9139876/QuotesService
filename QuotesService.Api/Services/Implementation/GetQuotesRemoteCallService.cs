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

        public async Task<List<QuoteModel>> GetQuotes(GetQuotesRequest request)
            => await ExecutePostAsync<List<QuoteModel>, GetQuotesRequest>("api/get-quotes", request);

        public async Task<QuotesInfoResponse> GetQuotesInfo(TickerMarketTimeFrame request)
            => await ExecutePostAsync<QuotesInfoResponse, TickerMarketTimeFrame>("api/get-quotes-info", request);
    }
}
