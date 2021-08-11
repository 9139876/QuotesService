using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Services;

namespace QuotesService.WebApi.Controllers
{
    public class PingTestApiController
    {
        private readonly IPingTestRemoteCallService _pingTestService;

        public PingTestApiController(
            IPingTestRemoteCallService pingTestService)
        {
            _pingTestService = pingTestService;
        }

        [HttpGet]
        [Route("api/ping-test/get")]
        public string Get()
        {
            return _pingTestService.PingKey;
        }
    }
}
