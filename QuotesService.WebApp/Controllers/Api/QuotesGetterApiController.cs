using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Models;
using QuotesService.DAL.Repositories;
using QuotesService.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuotesService.WebApp.Controllers.Api
{
    [Route("quotes-getter-api")]
    public class QuotesGetterApiController : Controller
    {
        private readonly IMarketsRepository _marketsRepository;
        private readonly ITickersRepository _tickersRepository;
        private readonly ITickersInfoesRepository _tickersInfoesRepository;
        private readonly ITickersInfoesNamesRepository _tickersInfoesNamesRepository;
        private readonly ITickerTFsRepository _tickerTFsRepository;
        private readonly IQuotesDbContext _quotesDbContext;

        public QuotesGetterApiController(
            IMarketsRepository marketsRepository,
            ITickersRepository tickersRepository,
            ITickersInfoesRepository tickersInfoesRepository,
            ITickersInfoesNamesRepository tickersInfoesNamesRepository,
            ITickerTFsRepository tickerTFsRepository,
            IQuotesDbContext quotesDbContext)
        {
            _marketsRepository = marketsRepository;
            _tickersRepository = tickersRepository;
            _tickersInfoesRepository = tickersInfoesRepository;
            _tickersInfoesNamesRepository = tickersInfoesNamesRepository;
            _tickerTFsRepository = tickerTFsRepository;
            _quotesDbContext = quotesDbContext;
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

            foreach (var ticker in tickers)
            {
                dict[ticker.MarketId].TickersNames.Add(ticker.Name);
            }

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

            var newTicker = new TickerEntity()
            {
                Name = request.TickerName,
                MarketId = existingMarket.Id
            };

            using (var transaction = _quotesDbContext.BeginTransaction())
            {
                await _tickersRepository.InsertAsync(newTicker);

                foreach (TimeFrameEnum tf in Enum.GetValues(typeof(TimeFrameEnum)))
                {
                    var ttf = new TickerTFEntity()
                    {
                        TickerId = newTicker.Id,
                        TimeFrame = tf
                    };

                    await _tickerTFsRepository.InsertAsync(ttf);
                }

                transaction.Commit();
            }

            return new StandartResponse()
            {
                IsSuccess = true
            };
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

        [HttpPost("get-ticker-info")]
        public async Task<TickerInfoResponse> GetTickerInfo([FromBody] TickerAndMarketRequest request)
        {
            request.RequiredNotNull(nameof(request));

            if (string.IsNullOrEmpty(request.MarketName) || string.IsNullOrEmpty(request.TickerName))
            {
                return new TickerInfoResponse()
                {
                    Status = new StandartResponse()
                    {
                        IsSuccess = false,
                        Message = $"Имя {(string.IsNullOrEmpty(request.MarketName) ? "рынка" : "инструмента") } не может быть пустым"
                    }
                };
            }

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(request);

            if (existingTicker == null)
            {
                return new TickerInfoResponse()
                {
                    Status = new StandartResponse()
                    {
                        IsSuccess = false,
                        Message = $"Инструмент {request.TickerName} для рынка {request.MarketName} не существует"
                    }
                };
            }

            var names = await _tickersInfoesNamesRepository.GetAllNames();

            var properties = (await _tickersInfoesRepository.GetTickerInfoesByTickerId(existingTicker.Id))
                .Where(x => names.Contains(x.KeyName))
                .ToList();

            foreach (var name in names.Where(x => properties.Select(x => x.KeyName).Contains(x) == false))
            {
                properties.Add(new TickerInfoEntity()
                {
                    TickerId = existingTicker.Id,
                    KeyName = name,
                    Value = null
                });
            }

            var result = new TickerInfoResponse()
            {
                MarketName = request.MarketName,
                TickerName = request.TickerName,
                Status = new StandartResponse()
                {
                    IsSuccess = true
                },
                Properties = properties.Select(x => new TickerInfoProperty()
                {
                    Name = x.KeyName,
                    Value = x.Value,
                    ReadOnly = false
                }).ToList()
            };

            return result;
        }

        [HttpPost("set-ticker-info")]
        public async Task<StandartResponse> SetTickerInfo([FromBody] TickerInfoResponse tickerInfo)
        {
            tickerInfo.RequiredNotNull(nameof(tickerInfo));

            if (string.IsNullOrEmpty(tickerInfo.MarketName) || string.IsNullOrEmpty(tickerInfo.TickerName))
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Имя {(string.IsNullOrEmpty(tickerInfo.MarketName) ? "рынка" : "инструмента") } не может быть пустым"
                };
            }

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(new TickerAndMarketRequest()
            {
                MarketName = tickerInfo.MarketName,
                TickerName = tickerInfo.TickerName
            });

            if (existingTicker == null)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Инструмент {tickerInfo.TickerName} для рынка {tickerInfo.MarketName} не существует"
                };
            }

            var names = await _tickersInfoesNamesRepository.GetAllNames();

            var existingProperties = await _tickersInfoesRepository.GetTickerInfoesByTickerId(existingTicker.Id);

            var forDelete = existingProperties.Where(x => names.Contains(x.KeyName) == false).ToList();

            var forInsert = new List<TickerInfoEntity>();
            var forUpdate = new List<TickerInfoEntity>();

            foreach (var name in names)
            {
                var newValue = tickerInfo.Properties.SingleOrDefault(x => x.Name == name);

                if (newValue != null)
                {
                    var oldValue = existingProperties.SingleOrDefault(x => x.KeyName == name);

                    if (oldValue == null)
                    {
                        forInsert.Add(new TickerInfoEntity()
                        {

                        });
                    }
                    else if (newValue.Value != oldValue.Value)
                    {
                        oldValue.Value = newValue.Value;
                        forUpdate.Add(oldValue);
                    }
                }
            }

            using (var transaction = _quotesDbContext.BeginTransaction())
            {
                foreach (var del in forDelete)
                {
                    await _tickersInfoesRepository.DeleteAsync(del);
                }

                foreach (var ins in forInsert)
                {
                    await _tickersInfoesRepository.InsertAsync(ins);
                }

                foreach (var upd in forUpdate)
                {
                    await _tickersInfoesRepository.UpdateAsync(upd);
                }
            }

            return new StandartResponse() { IsSuccess = true };
        }
    }
}
