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
    [Route("api/get")]
    [ApiController]
    public class GetDataController
    {
        private readonly IStrategyService _strategyService;
        private readonly IMarketsRepository _marketEntityRepository;

        public GetDataController(
            IStrategyService strategyService,
            IMarketsRepository marketEntityRepository)
        {
            _strategyService = strategyService;
            _marketEntityRepository = marketEntityRepository;
        }

        [HttpPost]
        [Route("quotes-check")]
        public async Task<CheckGetQuotesResponse> CheckGetQuotes([FromBody] CheckGetQuotesRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProvider);

            return await service.CheckGetQuotes(request);
        }

        [HttpGet]
        [Route("get-all-markets-names")]
        public async Task<List<string>> GetAllMarketsNames()
        {
            return await _marketEntityRepository.GetAllMarketsNames();
        }
    }
}
