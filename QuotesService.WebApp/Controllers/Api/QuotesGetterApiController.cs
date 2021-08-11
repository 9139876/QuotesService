using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Services;
using QuotesService.DAL.Models;
using QuotesService.DAL.Repositories;
using QuotesService.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuotesService.WebApp.Controllers.Api
{
    [Route("quotes-getter-api")]
    public class QuotesGetterApiController : Controller
    {
        private readonly IMarketsRepository _marketsRepository;
        private readonly ITickersRepository _tickersRepository;

        public QuotesGetterApiController(
            IMarketsRepository marketsRepository,
            ITickersRepository tickersRepository)
        {
            _marketsRepository = marketsRepository;
            _tickersRepository = tickersRepository;
        }

        [HttpGet("get-markets")]
        public async Task<List<MarketWithTickers>> GetMarkets()
        {
            var markets = await _marketsRepository.GetAllMarkets();
            var tickers = await _tickersRepository.GetAllTickers();

            var dict = new Dictionary<int, MarketWithTickers>();

            foreach (var market in markets)
            {
                dict.Add(market.Id, new MarketWithTickers() { MarketName = market.Name });
            }

            foreach(var ticker in tickers)
            {
                dict[ticker.MarketId].TickersNames.Add(ticker.Name);
            }

            ///!!! добавить тикер инфо

            return dict.Values.ToList();
        }

        [HttpGet("add-market")]
        public async Task<StandartResponse> AddMarket(string marketName)
        {
            if (string.IsNullOrEmpty(marketName))
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = "Имя рынка не может быть пустым"
                };
            }

            var existingMarket = await _marketsRepository.GetMarketByName(marketName);

            if (existingMarket != null)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Рынок с именем {marketName} уже существует"
                };
            }

            await _marketsRepository.InsertAsync(new DAL.Entities.MarketEntity() { Name = marketName });

            return new StandartResponse() { IsSuccess = true };
        }

        [HttpGet("delete-market")]
        public async Task<StandartResponse> DeleteMarket(string marketName)
        {
            if (string.IsNullOrEmpty(marketName))
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = "Имя рынка не может быть пустым"
                };
            }

            var existingMarket = await _marketsRepository.GetMarketByName(marketName);

            if (existingMarket == null)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Рынка с именем {marketName} не существует"
                };
            }

            await _marketsRepository.DeleteAsync(existingMarket);

            return new StandartResponse() { IsSuccess = true };
        }

        [HttpPost("add-ticker")]
        public async Task<StandartResponse> AddTicker([FromBody] TickerAndMarketRequest request)
        {
            request.RequiredNotNull(nameof(request));

            if (string.IsNullOrEmpty(request.MarketName) || string.IsNullOrEmpty(request.TickerName))
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Имя {(string.IsNullOrEmpty(request.MarketName) ? "рынка" : "инструмента") } не может быть пустым"
                };
            }

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(request);

            if (existingTicker != null)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Инструмент {request.TickerName} для рынка {request.MarketName} уже существует"
                };
            }

            var existingMarket = await _marketsRepository.GetMarketByName(request.MarketName);

            if (existingMarket == null)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Рынка с именем {request.MarketName} не существует"
                };
            }

            //!!! Добавить поля
            var newTicker = new DAL.Entities.TickerEntity()
            {
                Name = request.TickerName,
                MarketId = existingMarket.Id,
                Type = TickerTypeEnum.Other
            };

            await _tickersRepository.InsertAsync(newTicker);

            return new StandartResponse() { IsSuccess = true };
        }

        [HttpPost("delete-ticker")]
        public async Task<StandartResponse> DeleteTicker([FromBody] TickerAndMarketRequest request)
        {
            request.RequiredNotNull(nameof(request));

            if (string.IsNullOrEmpty(request.MarketName) || string.IsNullOrEmpty(request.TickerName))
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Имя {(string.IsNullOrEmpty(request.MarketName) ? "рынка" : "инструмента") } не может быть пустым"
                };
            }

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(request);

            if (existingTicker == null)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Инструмент {request.TickerName} для рынка {request.MarketName} не существует"
                };
            }

            await _tickersRepository.DeleteAsync(existingTicker);

            return new StandartResponse() { IsSuccess = true };
        }
    }
}
