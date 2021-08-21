using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Core.Extensions;
using Newtonsoft.Json.Linq;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.BL.Static;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Repositories;

namespace QuotesService.BL.Services.Implementation
{
    internal class YahooFinanceService : IYahooFinanceService
    {
        private static readonly DateTime _zeroDt = new DateTime(1970, 1, 1);
        private readonly ITickersRepository _tickersRepository;
        private readonly IQuotesProvidersRepository _quotesProvidersRepository;

        public YahooFinanceService(
            ITickersRepository tickersRepository,
            IQuotesProvidersRepository quotesProvidersRepository)
        {
            _tickersRepository = tickersRepository;
            _quotesProvidersRepository = quotesProvidersRepository;
        }

        public async Task<GetQuotesResponse> GetQuotes(GetQuotesRequest request)
        {
            var ticker = await GetTicker(request.TickerName, request.MarketName);

            var getDataInfo = ticker.ProviderGetDataInfo?.Deserialize<YahooFinanceGetDataInfoModel>();

            if (getDataInfo == null)
            {
                throw new InvalidOperationException($"Ticker not have {nameof(getDataInfo)} - {ticker.Serialize()}");
            }

            var url = GetQuotesURL(getDataInfo.Symbol, request.StartDate, request.EndDate, request.TimeFrame);

            return await GetQuotes(url);
        }

        public async Task<CheckGetQuotesResponse> CheckGetQuotes(CheckGetQuotesRequest request)
        {
            try
            {
                if (request.Parameters.Where(x => x.Key == nameof(YahooFinanceGetDataInfoModel.Symbol)).Any() == false)
                {
                    throw new InvalidOperationException();
                }

                var symbol = request.Parameters.Single(x => x.Key == nameof(YahooFinanceGetDataInfoModel.Symbol)).Value;

                var url = GetQuotesURL(symbol, request.StartDate, request.StartDate.AddMonths(1), TimeFrameEnum.D1);

                var quotes = await GetQuotes(url);

                if (quotes.Quotes.Any())
                {
                    return new CheckGetQuotesResponse()
                    {
                        IsSuccess = true,
                        StartDate = quotes.Quotes.Select(x => x.Date).Min()
                    };
                }
                else
                {
                    return new CheckGetQuotesResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Для начальной даты {request.StartDate.ToString("d")} в течение месяца не было ни одной котировки"
                    };
                }
            }
            catch (Exception e)
            {
                return new CheckGetQuotesResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request)
        {
            var ticker = await GetTicker(request.TickerName, request.MarketName);

            var getDataInfo = ticker.ProviderGetDataInfo?.Deserialize<YahooFinanceGetDataInfoModel>() ?? new YahooFinanceGetDataInfoModel();

            return ModelPropertiesCollectionConverter.ModelToPropertiesCollection(getDataInfo);
        }

        public async Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request)
        {
            try
            {
                var quotesProviderId = (await _quotesProvidersRepository.GetQuotesProviderByType(request.QuotesProvider))?.Id
                    ?? throw new InvalidOperationException($"QuotesProvider {request.QuotesProvider.ToString()} not exist in DB");

                var ticker = await GetTicker(request.TickerName, request.MarketName);

                if (ticker.QuotesProviderId != null && ticker.QuotesProviderId != quotesProviderId)
                {
                    var existingQuotesProvider = await _quotesProvidersRepository.GetQuotesProviderById((int)ticker.QuotesProviderId);
                    throw new InvalidOperationException($"Для тикера {request.TickerName} рынок {request.MarketName} уже установлен другой поставщик котировок - {existingQuotesProvider?.Name}");
                }

                var getDataInfo = ModelPropertiesCollectionConverter.PropertiesCollectionToModel<YahooFinanceGetDataInfoModel>(request.Parameters).Serialize();

                if (ticker.ProviderGetDataInfo != getDataInfo)
                {
                    ticker.ProviderGetDataInfo = getDataInfo;
                    await _tickersRepository.UpdateAsync(ticker);
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

        #region private static

        private static async Task<GetQuotesResponse> GetQuotes(string url)
        {
            var response = new GetQuotesResponse();

            using (var reader = new System.IO.StreamReader(new WebClient().OpenRead(url) ?? throw new InvalidOperationException($"Не удалось получить данные по адресу '{url}'")))
            {
                var data = await reader.ReadToEndAsync();
                response.Quotes = ParseQuotes(data);
            }

            return response;
        }

        private static string GetQuotesURL(string tickerSymbol, DateTime start, DateTime end, TimeFrameEnum timeFrame)
        {
            start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            end = new DateTime(end.Year, end.Month, end.Day, 20, 0, 0);

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
            var tickerAndMarketRequest = new TickerAndMarketRequest()
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
