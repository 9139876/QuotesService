using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.BL.Static;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.BL.Services.Implementation
{
    internal class YahooFinanceQuotesProviderService : AbstractQuotesProvider<YahooFinanceGetDataInfoModel>, IYahooFinanceQuotesProviderService
    {
        private static readonly DateTime _zeroDt = new(1970, 1, 1);
        private readonly IQuotesRepository _quotesRepository;

        public YahooFinanceQuotesProviderService(
            ITickersRepository tickersRepository,
            ITickerTFsRepository tickerTFsRepository,
            IQuotesProvidersTasksRepository quotesProvidersTasksRepository,
            IQuotesDbContext quotesDbContext,
            IQuotesRepository quotesRepository) : base(
                tickersRepository,
                tickerTFsRepository,
                quotesProvidersTasksRepository,
                quotesDbContext)
        {
            _quotesRepository = quotesRepository;
        }

        protected override QuotesProviderTypeEnum QuotesProviderType => QuotesProviderTypeEnum.YahooFinance;

        public override List<TimeFrameEnum> GetAvailableTimeFrames()
        {
            return new List<TimeFrameEnum>
            {
                TimeFrameEnum.D1,
                TimeFrameEnum.W1,
                TimeFrameEnum.M1
            };
        }

        public override async Task<GetQuotesResponse> GetLastBatchQuotes(TickerMarketTimeFrame request)
        {
            var lastQuote = await _quotesRepository.GetLastQuote(request);

            var getQuotesRequest = new GetQuotesWithQPRequest()
            {
                QuotesProvider = QuotesProviderType,
                TickerMarketTimeFrame = request
            };

            if (lastQuote != null)
            {
                getQuotesRequest.StartDate = lastQuote.Date;
            }
            else
            {
                getQuotesRequest.StartDate = new DateTime(1900, 1, 1);
            }

            getQuotesRequest.EndDate = AuxiliaryBL.GetPossibleEndDate(request.TimeFrame);

            var getQuotesResponse = await GetQuotes(getQuotesRequest);

            var result = new GetQuotesResponse() { Quotes = AuxiliaryBL.CorrectQuotes(new QuotesCorrectRequest() { TimeFrame = request.TimeFrame, Quotes = getQuotesResponse }) };

            return result;
        }

        protected override YahooFinanceGetDataInfoModel CreateGetDataInfoModel(List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Where(x => x.Key == nameof(YahooFinanceGetDataInfoModel.Symbol)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(YahooFinanceGetDataInfoModel.Symbol)}");
            }

            return new YahooFinanceGetDataInfoModel()
            {
                Symbol = parameters.Single(x => x.Key == nameof(YahooFinanceGetDataInfoModel.Symbol)).Value
            };
        }

        protected override string GetQuotesURL(YahooFinanceGetDataInfoModel getDataInfo, DateTime? start, DateTime? end, TimeFrameEnum timeFrame)
        {
            if (start == null)
            {
                throw new ArgumentException($"Date {nameof(start)} is null");
            }

            if (end == null)
            {
                throw new ArgumentException($"Date {nameof(end)} is null");
            }

            start = new DateTime(start.Value.Year, start.Value.Month, start.Value.Day, 0, 0, 0);
            end = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, 23, 59, 59);

            return $"https://query1.finance.yahoo.com/v7/finance/download/{getDataInfo.Symbol}?period1={GetDateValue(start.Value)}&period2={GetDateValue(end.Value)}&interval={GetTimeFrameCode(timeFrame)}&events=history&includeAdjustedClose=true";
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
        private static long GetDateValue(DateTime dt)
        {
            return (long)((dt - _zeroDt).TotalSeconds);
        }

        protected override List<QuoteModel> ParseQuotes(string data)
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
    }
}
