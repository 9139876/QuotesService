using CommonLibraries.RemoteCall.Services;
using CommonLibraries.RemoteCall.Services.Implementation;
using Microsoft.Extensions.Configuration;

namespace QuotesService.Api.Services.Implementation
{
    internal class PingTestRemoteCallService : BasePingTestService, IPingTestRemoteCallService
    {
        public PingTestRemoteCallService(IConfiguration configuration, IRemoteCallHelperService remoteCallHelperService) : base(configuration, remoteCallHelperService)
        {
        }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "QuotesService.Api.SchemeAndHost";
        protected override string _pingKey => "QuotesService.Api";
        protected override string _path => "api/ping-test/get";
    }
}
