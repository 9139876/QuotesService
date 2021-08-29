using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.RemoteCall;
using CommonLibraries.RemoteCall.Services;
using Microsoft.Extensions.Configuration;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.ApiPrivate.Models;
using QuotesService.ApiPrivate.Models.RequestResponse;

namespace QuotesService.ApiPrivate.Services.Implementation
{
    class QuotesProviderRemoteCallService : BaseRemoteCallService, IQuotesProviderRemoteCallService
    {
        public QuotesProviderRemoteCallService(
            IConfiguration configuration,
            IRemoteCallHelperService remoteCallHelperService) : base(configuration, remoteCallHelperService) { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "QuotesService.ApiPrivate.SchemeAndHost";

        public async Task<StandartResponse> CheckGetQuotes(CheckGetQuotesRequest request)
            => await ExecutePostAsync<StandartResponse, CheckGetQuotesRequest>("api/quotes-provider/check-get-quotes", request);

        public async Task<GetQuotesResponse> GetQuotes(GetQuotesWithQPRequest request)
            => await ExecutePostAsync<GetQuotesResponse, GetQuotesWithQPRequest>("api/quotes-provider/get-quotes", request);

        public async Task<GetQuotesProviderResponse> GetQuotesProvider(TickerAndMarket request)
            => await ExecutePostAsync<GetQuotesProviderResponse, TickerAndMarket>("api/quotes-provider/get-quotes-provider", request);

        public async Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request)
            => await ExecutePostAsync<List<KeyValuePair<string, string>>, GetQuotesProviderParametersRequest>("api/quotes-provider/get-quotes-provider-parameters", request);

        public async Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request)
            => await ExecutePostAsync<StandartResponse, SetQuotesProviderParametersRequest>("api/quotes-provider/set-quotes-provider-parameters", request);
    }
}
