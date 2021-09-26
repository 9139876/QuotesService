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
using System.Threading.Tasks;

namespace QuotesService.BL.Services.Implementation
{
    internal class StooqService : AbstractQuotesProvider<StooqGetDataInfoModel>, IStooqService
    {
        private readonly IQuotesRepository _quotesRepository;

        protected override QuotesProviderEnum QuotesProviderType => QuotesProviderEnum.Stooq;

        public StooqService(
            ITickersRepository tickersRepository,
            IQuotesProvidersRepository quotesProvidersRepository,
            ITickerTFsRepository tickerTFsRepository,
            IQuotesProvidersTasksRepository quotesProvidersTasksRepository,
            IQuotesDbContext quotesDbContext,
            IQuotesRepository quotesRepository) : base(
                tickersRepository,
                quotesProvidersRepository,
                tickerTFsRepository,
                quotesProvidersTasksRepository,
                quotesDbContext)
        {
            _quotesRepository = quotesRepository;
        }

        public override List<TimeFrameEnum> GetAvailableTimeFrames()
        {
            return new List<TimeFrameEnum>
            {
                TimeFrameEnum.D1,
                TimeFrameEnum.W1,
                TimeFrameEnum.M1,
                TimeFrameEnum.Seasonly,
                TimeFrameEnum.Y1
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
                getQuotesRequest.EndDate = Auxiliary.GetPossibleEndDate(request.TimeFrame);
            }
            else
            {
                getQuotesRequest.StartDate = null;
                getQuotesRequest.EndDate = null;
            }

            var getQuotesResponse = await GetQuotes(getQuotesRequest);

            var result = new GetQuotesResponse() { Quotes = Auxiliary.CorrectQuotes(new QuotesCorrectRequest() { TimeFrame = request.TimeFrame, Quotes = getQuotesResponse }) };

            return result;
        }

        protected override StooqGetDataInfoModel CreateGetDataInfoModel(List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Where(x => x.Key == nameof(StooqGetDataInfoModel.Symbol)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(StooqGetDataInfoModel.Symbol)}");
            }

            return new StooqGetDataInfoModel()
            {
                Symbol = parameters.Single(x => x.Key == nameof(YahooFinanceGetDataInfoModel.Symbol)).Value
            };
        }

        protected override string GetQuotesURL(StooqGetDataInfoModel getDataInfo, DateTime? start, DateTime? end, TimeFrameEnum timeFrame)
        {            
            if (string.IsNullOrEmpty(getDataInfo?.Symbol))
            {
                throw new ArgumentException($"Value {nameof(getDataInfo.Symbol)} is null or empty");
            }

            var tfCode = GetTimeFrameCode(timeFrame);

            if (start == null && end == null)
            {
                return $"https://stooq.com/q/d/l/?s={getDataInfo.Symbol}&i={tfCode}";
            }

            if(start == null)
            {
                throw new ArgumentException($"Date {nameof(start)} is null");
            }

            if (end == null)
            {
                throw new ArgumentException($"Date {nameof(end)} is null");
            }

            var d1 = start.Value.ToString("yyyyMMdd");
            var d2 = end.Value.ToString("yyyyMMdd");

            return $"https://stooq.com/q/d/l/?s={getDataInfo.Symbol}&d1={d1}&d2={d2}&i={tfCode}";
            //https://stooq.com/q/d/l/?s=aapl.us&d1=20000907&d2=20210924&i=d

        }

        private static string GetTimeFrameCode(TimeFrameEnum timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrameEnum.D1: return "d";
                case TimeFrameEnum.W1: return "w";
                case TimeFrameEnum.M1: return "m";
                case TimeFrameEnum.Seasonly: return "q";
                case TimeFrameEnum.Y1: return "y";

                default:
                    throw new ArgumentOutOfRangeException($"Неподходящий таймфрейм для Stooq - {timeFrame.ToString()}");
            }
        }

        protected override List<QuoteModel> ParseQuotes(string data)
        {
            var result = new List<QuoteModel>();

            foreach (var row in data.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                var values = row.Split(',');

                result.Add(new QuoteModel()
                {
                    Date = DateTime.Parse(values[0]),
                    Open = decimal.Parse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Hi = decimal.Parse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Low = decimal.Parse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Close = decimal.Parse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Volume = decimal.Parse(values[5], NumberStyles.Any, CultureInfo.InvariantCulture),
                });
            }

            return result;
        }
    }
}
