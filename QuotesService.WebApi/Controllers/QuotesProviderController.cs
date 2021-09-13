using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.ApiPrivate.Models;
using QuotesService.ApiPrivate.Models.RequestResponse;
using QuotesService.BL.Services;
using QuotesService.DAL.Repositories;

namespace QuotesService.WebApi.Controllers
{
    [Route("api/quotes-provider")]
    [ApiController]
    public class QuotesProviderController
    {
        private readonly IStrategyService _strategyService;
        private readonly ITickersRepository _tickersRepository;
        private readonly IQuotesProvidersRepository _quotesProvidersRepository;

        public QuotesProviderController(
            IStrategyService strategyService,
            ITickersRepository tickersRepository,
            IQuotesProvidersRepository quotesProvidersRepository)
        {
            _strategyService = strategyService;
            _tickersRepository = tickersRepository;
            _quotesProvidersRepository = quotesProvidersRepository;
        }

        [HttpPost]
        [Route("check-get-quotes")]
        public async Task<StandartResponse> CheckGetQuotes([FromBody] CheckGetQuotesRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProviderType);

            return await service.CheckGetQuotes(request);
        }

        [HttpPost]
        [Route("get-quotes-provider")]
        public async Task<GetQuotesProviderResponse> GetQuotesProvider([FromBody] TickerAndMarket request)
        {
            request.RequiredNotNull(nameof(request));

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(request);

            if (existingTicker == null)
            {
                throw new InvalidOperationException($"Ticker {request.TickerName} in market {request.MarketName} not exsit");
            }

            var allQuotesProviders = await _quotesProvidersRepository.GetAllQuotesProviders();

            var result = new GetQuotesProviderResponse()
            {
                AllQuotesProviders = allQuotesProviders.Select(x => new QuotesProvider()
                {
                    QuotesProviderName = x.Name,
                    QuotesProviderType = x.QuotesProviderType
                }).ToList()
            };

            if (existingTicker.QuotesProviderId > 0)
            {
                var currentQuotesProvider = await _quotesProvidersRepository.GetQuotesProviderById(existingTicker.QuotesProviderId.Value);
                currentQuotesProvider.RequiredNotNull(nameof(currentQuotesProvider), existingTicker.QuotesProviderId);

                result.QuotesProviderAssigned = true;

                result.CurrentQuotesProvider = new QuotesProvider()
                {
                    QuotesProviderName = currentQuotesProvider.Name,
                    QuotesProviderType = currentQuotesProvider.QuotesProviderType
                };
            }

            return result;
        }

        [HttpPost]
        [Route("get-quotes-provider-parameters")]
        public async Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters([FromBody] GetQuotesProviderParametersRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProviderType);

            return await service.GetQuotesProviderParameters(request);
        }

        [HttpPost]
        [Route("set-quotes-provider-parameters")]
        public async Task<StandartResponse> SetQuotesProviderParameters([FromBody] SetQuotesProviderParametersRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProviderType);

            return await service.SetQuotesProviderParameters(request);
        }
    }
}
