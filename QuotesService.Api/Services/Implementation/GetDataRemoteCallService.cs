using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.RemoteCall;
using CommonLibraries.RemoteCall.Services;
using Microsoft.Extensions.Configuration;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;

namespace QuotesService.Api.Services.Implementation
{
    class GetDataRemoteCallService : BaseRemoteCallService, IGetDataRemoteCallService
    {
        public GetDataRemoteCallService(
            IConfiguration configuration,
            IRemoteCallHelperService remoteCallHelperService) : base(configuration, remoteCallHelperService) { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "QuotesService.Api.SchemeAndHost";

        public async Task<CheckGetQuotesResponse> CheckGetQuotes(CheckGetQuotesRequest request)
            => await ExecutePostAsync<CheckGetQuotesResponse, CheckGetQuotesRequest>("api/quotes-provider/check-get-quotes", request);

        public async Task<GetQuotesResponse> GetQuotes(GetQuotesRequest request)
            => await ExecutePostAsync<GetQuotesResponse, GetQuotesRequest>("api/quotes-provider/get-quotes", request);

        public async Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request)
        => await ExecutePostAsync<List<KeyValuePair<string, string>>, GetQuotesProviderParametersRequest>("api/quotes-provider/get-quotes-privider-parameters", request);

        public async Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request)
        => await ExecutePostAsync<StandartResponse, SetQuotesProviderParametersRequest>("api/quotes-provider/set-quotes-privider-parameters", request);
    }
}
