using CommonLibraries.Core.Extensions;
using CommonLibraries.Core.Models;
using CommonLibraries.Graal.Enums;
using CommonLibraries.Graal.Models;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.Api.Services;
using QuotesService.BL.Models;
using QuotesService.BL.Services;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Models;
using QuotesService.DAL.Repositories;
using QuotesService.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IStrategyService _strategyService;
        private readonly IGetQuotesRemoteCallService _getQuotesRemoteCallService;
        private readonly IQuotesStorageService _quotesStorageService;

        public QuotesGetterApiController(
            IMarketsRepository marketsRepository,
            ITickersRepository tickersRepository,
            ITickersInfoesRepository tickersInfoesRepository,
            ITickersInfoesNamesRepository tickersInfoesNamesRepository,
            ITickerTFsRepository tickerTFsRepository,
            IQuotesDbContext quotesDbContext,
            IStrategyService strategyService,
            IGetQuotesRemoteCallService getQuotesRemoteCallService,
            IQuotesStorageService quotesStorageService)
        {
            _marketsRepository = marketsRepository;
            _tickersRepository = tickersRepository;
            _tickersInfoesRepository = tickersInfoesRepository;
            _tickersInfoesNamesRepository = tickersInfoesNamesRepository;
            _tickerTFsRepository = tickerTFsRepository;
            _quotesDbContext = quotesDbContext;
            _strategyService = strategyService;
            _getQuotesRemoteCallService = getQuotesRemoteCallService;
            _quotesStorageService = quotesStorageService;
        }

        [HttpGet("get-markets")]
        public async Task<List<MarketWithTickers>> GetMarkets()
        {
            var markets = await _marketsRepository.GetAllMarkets();
            var tickers = await _tickersRepository.GetAllTickers();

            var dict = new Dictionary<int, MarketWithTickers>();

            foreach (var market in markets.OrderBy(x => x.Name))
            {
                dict.Add(market.Id, new MarketWithTickers() { MarketName = market.Name });
            }

            foreach (var ticker in tickers.OrderBy(x => x.Name))
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
        public async Task<StandartResponse> AddTicker([FromBody] TickerAndMarket request)
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
        public async Task<StandartResponse> DeleteTicker([FromBody] TickerAndMarket request)
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
        public async Task<TickerInfoModel> GetTickerInfo([FromBody] TickerAndMarket request)
        {
            request.RequiredNotNull(nameof(request));

            if (string.IsNullOrEmpty(request.MarketName) || string.IsNullOrEmpty(request.TickerName))
            {
                return new TickerInfoModel()
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
                return new TickerInfoModel()
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

            var result = new TickerInfoModel()
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

            string quotesProviderName = null;

            if (existingTicker.QuotesProviderType != null)
            {
                quotesProviderName = existingTicker.QuotesProviderType.ToString();
            }

            result.Properties.Add(new TickerInfoProperty()
            {
                Name = "Поставщик котировок",
                Value = quotesProviderName,
                ReadOnly = true
            });

            return result;
        }

        [HttpPost("set-ticker-info")]
        public async Task<StandartResponse> SetTickerInfo([FromBody] TickerInfoModel tickerInfo)
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

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(new TickerAndMarket()
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
                            TickerId = existingTicker.Id,
                            KeyName = name,
                            Value = newValue.Value
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

                transaction.Commit();
            }

            return new StandartResponse() { IsSuccess = true };
        }

        [HttpPost("check-get-quotes")]
        public async Task<StandartResponse> CheckGetQuotes([FromBody] CheckGetQuotesRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProviderType);

            return await service.CheckGetQuotes(request);
        }

        [HttpPost("get-quotes-provider")]
        public async Task<GetQuotesProviderResponse> GetQuotesProvider([FromBody] TickerAndMarket request)
        {
            request.RequiredNotNull(nameof(request));

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(request);

            if (existingTicker == null)
            {
                throw new InvalidOperationException($"Ticker {request.TickerName} in market {request.MarketName} not exsit");
            }

            var result = new GetQuotesProviderResponse()
            {
                AllQuotesProviders = Enum.GetValues<QuotesProviderTypeEnum>().Select(x => new QuotesProvider()
                {
                    QuotesProviderName = x.ToString(),
                    QuotesProviderType = x
                }).ToList()
            };

            if (existingTicker.QuotesProviderType != null)
            {
                result.QuotesProviderAssigned = true;

                result.CurrentQuotesProvider = new QuotesProvider()
                {
                    QuotesProviderName = existingTicker.QuotesProviderType.ToString(),
                    QuotesProviderType = existingTicker.QuotesProviderType.Value
                };
            }

            return result;
        }

        [HttpPost("get-quotes-provider-parameters")]
        public async Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters([FromBody] GetQuotesProviderParametersRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var service = _strategyService.GetInstance(request.QuotesProviderType);

            return await service.GetQuotesProviderParameters(request);
        }

        [HttpPost("set-quotes-provider-parameters")]
        public async Task<StandartResponse> SetQuotesProviderParameters([FromBody] SetQuotesProviderParametersRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var check = await CheckGetQuotes(new CheckGetQuotesRequest()
            {
                QuotesProviderType = request.QuotesProviderType,
                Parameters = request.Parameters
            });

            if (check.IsSuccess != true)
            {
                return check;
            }

            var service = _strategyService.GetInstance(request.QuotesProviderType);

            return await service.SetQuotesProviderParameters(request);
        }

        [HttpPost("get-quotes-info")]
        public async Task<List<QuotesInfo>> GetQuotesInfo([FromBody] TickerAndMarket request)
        {
            request.RequiredNotNull(nameof(request));

            var result = new List<QuotesInfo>();

            foreach (TimeFrameEnum tf in Enum.GetValues(typeof(TimeFrameEnum)))
            {
                var getQuotesInfoRequest = new TickerMarketTimeFrame()
                {
                    TimeFrame = tf,
                    MarketName = request.MarketName,
                    TickerName = request.TickerName
                };

                var quotesInfo = await _getQuotesRemoteCallService.GetQuotesInfo(getQuotesInfoRequest);
                quotesInfo.RequiredNotNull(nameof(quotesInfo), getQuotesInfoRequest);

                result.Add(quotesInfo);
            }

            return result;
        }

        [HttpPost("get-quotes-info-by-tf")]
        public async Task<QuotesInfo> GetQuotesInfoByTf([FromBody] TickerMarketTimeFrame request)
        {
            request.RequiredNotNull(nameof(request));

            var result = await _getQuotesRemoteCallService.GetQuotesInfo(request);
            result.RequiredNotNull(nameof(result), request);

            return result;
        }

        [HttpGet("get-timeframes")]
        public List<TimeFramesModel> GetTimeFrames()
        {
            return Enum.GetValues<TimeFrameEnum>()
                .Select(x => new TimeFramesModel()
                {
                    Type = x.ToString(),
                    Name = Description.GetDescription(x)
                })
                .ToList();
        }

        [HttpPost("compare-quotes")]
        public async Task<CompareQuotesResponse> CompareQuotes([FromBody] CompareQuotesRequest request)
        {
            request.RequiredNotNull(nameof(request));

            return await _quotesStorageService.CompareQuotes(request);
        }
    }
}
