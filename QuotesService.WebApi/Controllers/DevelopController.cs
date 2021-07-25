using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Services;

namespace QuotesService.WebApi.Controllers
{
    public class DevelopController
    {
        private readonly IYahooFinanceService _yahooFinanceService;

        public DevelopController(
            IYahooFinanceService yahooFinanceService)
        {
            _yahooFinanceService = yahooFinanceService;
        }

        [HttpPost]
        [Route("api/develop/get-yahoo-ti")]
        public async Task<TryGetTickerInfoYFFromServerResponse> GetYahooTi([FromBody] TryGetTickerInfoYFFromServerRequest request)
        {
            request.RequiredNotNull(nameof(request));
            return await _yahooFinanceService.TryGetTickerInfoFromServerAsync(request);
        }
    }
}
