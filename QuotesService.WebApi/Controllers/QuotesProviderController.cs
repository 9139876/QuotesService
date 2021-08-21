using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Services;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Repositories;

namespace QuotesService.WebApi.Controllers
{
    [Route("api/quotes-provider")]
    [ApiController]
    public class QuotesProviderController
    {
        private readonly IStrategyService _strategyService;
        private readonly IMarketsRepository _marketEntityRepository;

        public QuotesProviderController(
            IStrategyService strategyService,
            IMarketsRepository marketEntityRepository)
        {
            _strategyService = strategyService;
            _marketEntityRepository = marketEntityRepository;
        }

        [HttpPost]
        [Route("check-get-quotes")]
        public async Task<CheckGetQuotesResponse> CheckGetQuotes([FromBody] CheckGetQuotesRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProvider);

            return await service.CheckGetQuotes(request);
        }

        [HttpPost]
        [Route("get-quotes")]
        public async Task<GetQuotesResponse> GetQuotes([FromBody] GetQuotesRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProvider);

            return await service.GetQuotes(request);
        }

        [HttpPost]
        [Route("get-quotes-privider-parameters")]
        public async Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters([FromBody] GetQuotesProviderParametersRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProvider);

            return await service.GetQuotesProviderParameters(request);
        }

        [HttpPost]
        [Route("set-quotes-privider-parameters")]
        public async Task<StandartResponse> SetQuotesProviderParameters([FromBody] SetQuotesProviderParametersRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProvider);

            return await service.SetQuotesProviderParameters(request);
        }
    }
}
