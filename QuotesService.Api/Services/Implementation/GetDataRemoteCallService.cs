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
            => await ExecutePostAsync<CheckGetQuotesResponse, CheckGetQuotesRequest>("api/get/quotes-check", request);

        public async Task<List<string>> GetAllMarketsNames()
            => await ExecuteGetAsync<List<string>>("api/get/get-markets-names");
    }
}
