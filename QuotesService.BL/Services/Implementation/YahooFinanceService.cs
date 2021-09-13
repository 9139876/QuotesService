using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLibraries.Core.Extensions;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.ApiPrivate.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.BL.Static;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Models;
using QuotesService.DAL.Repositories;

namespace QuotesService.BL.Services.Implementation
{
    internal class YahooFinanceService : IYahooFinanceService
    {
        private static readonly DateTime _zeroDt = new DateTime(1970, 1, 1);
        private readonly ITickersRepository _tickersRepository;
        private readonly IQuotesProvidersRepository _quotesProvidersRepository;
        private readonly ITickerTFsRepository _tickerTFsRepository;
        private readonly IQuotesProvidersTasksRepository _quotesProvidersTasksRepository;
        private readonly IQuotesRepository _quotesRepository;
        private readonly IQuotesDbContext _quotesDbContext;

        public YahooFinanceService(
            ITickersRepository tickersRepository,
            IQuotesProvidersRepository quotesProvidersRepository,
            ITickerTFsRepository tickerTFsRepository,
            IQuotesProvidersTasksRepository quotesProvidersTasksRepository,
            IQuotesRepository quotesRepository,
            IQuotesDbContext quotesDbContext
            )
        {
            _tickersRepository = tickersRepository;
            _quotesProvidersRepository = quotesProvidersRepository;
            _tickerTFsRepository = tickerTFsRepository;
            _quotesProvidersTasksRepository = quotesProvidersTasksRepository;
            _quotesRepository = quotesRepository;
            _quotesDbContext = quotesDbContext;
        }

        public async Task<GetQuotesResponse> GetLastBatchQuotes(TickerMarketTimeFrame request)
        {            
            var lastQuote = await _quotesRepository.GetLastQuote(request);

            var getQuotesRequest = new GetQuotesWithQPRequest()
            {
                QuotesProvider = QuotesProviderEnum.YahooFinance,
                TickerMarketTimeFrame = request
            };

            if (lastQuote != null)
            {
                getQuotesRequest.StartDate = lastQuote.Date;
            }
            else
            {
                getQuotesRequest.StartDate = await Auxiliary.SearchFirstDate(request, this.GetQuotes);
            }

            getQuotesRequest.EndDate = Auxiliary.GetEndBatchDate(getQuotesRequest.StartDate, request.TimeFrame);

            var getQuotesResponse = await GetQuotes(getQuotesRequest);

            var result = new GetQuotesResponse() { Quotes = Auxiliary.CorrectQuotes(new QuotesCorrectRequest() { TimeFrame = request.TimeFrame, Quotes = getQuotesResponse }) };

            return result;
            ;
        }

        public async Task<StandartResponse> CheckGetQuotes(CheckGetQuotesRequest request)
        {
            try
            {
                if (request.Parameters.Where(x => x.Key == nameof(YahooFinanceGetDataInfoModel.Symbol)).Any() == false)
                {
                    throw new InvalidOperationException($"Пропущен обязяательный параметр - {nameof(YahooFinanceGetDataInfoModel.Symbol)}");
                }

                var symbol = request.Parameters.Single(x => x.Key == nameof(YahooFinanceGetDataInfoModel.Symbol)).Value;

                var url = GetQuotesURL(symbol, DateTime.Now.AddMonths(-1), DateTime.Now, TimeFrameEnum.D1);

                var quotes = await GetQuotes(url);

                if (quotes.Any())
                {
                    return new StandartResponse()
                    {
                        IsSuccess = true
                    };
                }
                else
                {
                    return new StandartResponse()
                    {
                        IsSuccess = false,
                        Message = $"В течение последнего месяца не было ни одной котировки"
                    };
                }
            }
            catch (Exception e)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request)
        {
            var ticker = await GetTicker(request.TickerName, request.MarketName);

            var getDataInfo = ticker.ProviderGetDataInfo?.Deserialize<YahooFinanceGetDataInfoModel>() ?? new YahooFinanceGetDataInfoModel();

            return getDataInfo.ToPropertiesCollection();
        }

        public async Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request)
        {
            try
            {
                var ticker = await GetTicker(request.TickerName, request.MarketName);

                if (ticker.QuotesProviderId != null)
                {
                    throw new InvalidOperationException($"Для инструмента {request.TickerName} рынок {request.MarketName} уже установлены параметры поставщика котировок, изменять их нельзя, нужно создать новый инструмент");
                }

                var quotesProvider = await _quotesProvidersRepository.GetQuotesProviderByType(request.QuotesProviderType);
                quotesProvider.RequiredNotNull(nameof(quotesProvider), request.QuotesProviderType);

                ticker.QuotesProviderId = quotesProvider.Id;
                ticker.ProviderGetDataInfo = request.Parameters.ToModel<YahooFinanceGetDataInfoModel>().Serialize();

                var tasks = new List<QuotesProviderTaskEntity>();

                foreach (var tf in GetAvailableTimeFrames())
                {
                    var ttf = await _tickerTFsRepository.GetByTickerIdAndTF(ticker.Id, tf);
                    ttf.RequiredNotNull(nameof(ttf), new { ticker.Id, tf });

                    tasks.Add(new QuotesProviderTaskEntity()
                    {
                        TickerTFId = ttf.Id,
                        UpdatePeriodInSecond = 3600,
                        IsActive = false
                    });
                }

                using (var transaction = _quotesDbContext.BeginTransaction())
                {
                    await _tickersRepository.UpdateAsync(ticker);

                    foreach (var task in tasks)
                    {
                        await _quotesProvidersTasksRepository.InsertAsync(task);
                    }

                    transaction.Commit();
                }

                return new StandartResponse() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }

        public List<TimeFrameEnum> GetAvailableTimeFrames()
        {
            return new List<TimeFrameEnum>
            {
                TimeFrameEnum.D1,
                TimeFrameEnum.W1,
                TimeFrameEnum.M1
            };
        }

        private async Task<List<QuoteModel>> GetQuotes(GetQuotesRequest request)
        {
            var ticker = await GetTicker(request.TickerMarketTimeFrame.TickerName, request.TickerMarketTimeFrame.MarketName);

            var getDataInfo = ticker.ProviderGetDataInfo?.Deserialize<YahooFinanceGetDataInfoModel>();

            if (getDataInfo == null)
            {
                throw new InvalidOperationException($"Ticker not have {nameof(getDataInfo)} - {ticker.Serialize()}");
            }

            var url = GetQuotesURL(getDataInfo.Symbol, request.StartDate, request.EndDate, request.TickerMarketTimeFrame.TimeFrame);

            return await GetQuotes(url);
        }

        #region private static

        private static async Task<List<QuoteModel>> GetQuotes(string url)
        {
            var data = string.Empty;

            using (var reader = new System.IO.StreamReader(new WebClient().OpenRead(url) ?? throw new InvalidOperationException($"Не удалось получить данные по адресу '{url}'")))
            {
                data = await reader.ReadToEndAsync();

            }
            var response = ParseQuotes(data);
            return response;
        }

        private static string GetQuotesURL(string tickerSymbol, DateTime start, DateTime end, TimeFrameEnum timeFrame)
        {
            start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

            return $"https://query1.finance.yahoo.com/v7/finance/download/{tickerSymbol}?period1={GetDateValue(start)}&period2={GetDateValue(end)}&interval={GetTimeFrameCode(timeFrame)}&events=history&includeAdjustedClose=true";
        }

        private static long GetDateValue(DateTime dt)
        {
            return (long)((dt - _zeroDt).TotalSeconds);
        }

        private static string GetTimeFrameCode(TimeFrameEnum timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrameEnum.D1: return "1d";
                case TimeFrameEnum.W1: return "1wk";
                case TimeFrameEnum.M1: return "1mo";

                default:
                    throw new ArgumentOutOfRangeException($"Неподходящий таймфрейм для YahooFinance - {timeFrame.ToString()}");
            }
        }

        private static List<QuoteModel> ParseQuotes(string data)
        {
            var result = new List<QuoteModel>();

            foreach (var row in data.Split('\n', StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                var values = row.Split(',');

                result.Add(new QuoteModel()
                {
                    Date = DateTime.Parse(values[0]),
                    Open = decimal.Parse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Hi = decimal.Parse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Low = decimal.Parse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Close = decimal.Parse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Volume = decimal.Parse(values[6], NumberStyles.Any, CultureInfo.InvariantCulture),
                });
            }

            return result;
        }

        private async Task<TickerEntity> GetTicker(string tickerName, string marketName)
        {
            var tickerAndMarketRequest = new TickerAndMarket()
            {
                MarketName = marketName,
                TickerName = tickerName
            };

            var ticker = await _tickersRepository.GetByTickerAndMarket(tickerAndMarketRequest);

            if (ticker == null)
            {
                throw new InvalidOperationException($"Ticker not found for {tickerAndMarketRequest.Serialize()}");
            }

            return ticker;
        }

        #endregion
    }
}
