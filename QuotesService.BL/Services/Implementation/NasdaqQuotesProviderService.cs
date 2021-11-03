using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.BL.Static;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Net;
using CommonLibraries.Graal.Enums;
using CommonLibraries.Graal.Models;
using CommonLibraries.Graal.Extensions;

namespace QuotesService.BL.Services.Implementation
{
    internal class NasdaqQuotesProviderService : AbstractQuotesProvider<NasdaqGetDataInfoModel>, INasdaqQuotesProviderService
    {
        private readonly IQuotesRepository _quotesRepository;

        public NasdaqQuotesProviderService(
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

        protected override QuotesProviderTypeEnum QuotesProviderType => QuotesProviderTypeEnum.Nasdaq;

        public override List<TimeFrameEnum> GetAvailableTimeFrames()
        {
            return new List<TimeFrameEnum>() { TimeFrameEnum.D1 };
        }

        public override async Task<GetQuotesResponse> GetLastBatchQuotes(TickerMarketTimeFrame request)
        {
            var lastQuote = await _quotesRepository.GetLastQuote(request);

            var getQuotesRequest = new GetQuotesWithQPRequest()
            {
                QuotesProvider = QuotesProviderType,
                TickerMarketTimeFrame = request,
                EndDate = DateTimeExtensions.GetPossibleEndDate(request.TimeFrame, DateTime.Now)
            };

            if (lastQuote != null)
            {
                getQuotesRequest.StartDate = lastQuote.Date;
            }
            else
            {
                getQuotesRequest.StartDate = getQuotesRequest.EndDate.Value.AddYears(-30);
            }

            var getQuotesResponse = await GetQuotes(getQuotesRequest);

            var result = new GetQuotesResponse() { Quotes = AuxiliaryBL.CorrectQuotes(new QuotesCorrectRequest() { TimeFrame = request.TimeFrame, Quotes = getQuotesResponse }) };

            return result;
        }

        protected override NasdaqGetDataInfoModel CreateGetDataInfoModel(List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Where(x => x.Key == nameof(NasdaqGetDataInfoModel.Class)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(NasdaqGetDataInfoModel.Class)}");
            }

            if (parameters.Where(x => x.Key == nameof(NasdaqGetDataInfoModel.Symbol)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(NasdaqGetDataInfoModel.Symbol)}");
            }

            return new NasdaqGetDataInfoModel()
            {
                Class = parameters.Single(x => x.Key == nameof(NasdaqGetDataInfoModel.Class)).Value,
                Symbol = parameters.Single(x => x.Key == nameof(NasdaqGetDataInfoModel.Symbol)).Value
            };
        }

        protected override string GetQuotesURL(NasdaqGetDataInfoModel getDataInfo, DateTime? start, DateTime? end, TimeFrameEnum timeFrame)
        {
            if (start == null)
            {
                throw new ArgumentException($"Date {nameof(start)} is null, but {nameof(end)} is not null");
            }
            else if (end == null)
            {
                throw new ArgumentException($"Date {nameof(end)} is null, but {nameof(start)} is not null");
            }

            var d1 = start.Value.ToString("yyyy-MM-dd");
            var d2 = end.Value.ToString("yyyy-MM-dd");

            return $"https://api.nasdaq.com/api/quote/{getDataInfo.Symbol}/historical?assetclass={getDataInfo.Class}&fromdate={d1}&limit=9999&todate={d2}";
        }

        protected override List<QuoteModel> ParseQuotes(string data)
        {
            var result = new List<QuoteModel>();

            dynamic d = JsonConvert.DeserializeObject<object>(data);

            foreach (var row in d.data.tradesTable.rows.ToObject<List<object>>())
            {
                result.Add(new QuoteModel()
                {
                    Date = DateTime.ParseExact(row.date.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Open = decimal.Parse(row.open.ToString().Trim('$'), CultureInfo.InvariantCulture),
                    Hi = decimal.Parse(row.high.ToString().Trim('$'), CultureInfo.InvariantCulture),
                    Low = decimal.Parse(row.low.ToString().Trim('$'), CultureInfo.InvariantCulture),
                    Close = decimal.Parse(row.close.ToString().Trim('$'), CultureInfo.InvariantCulture),
                    Volume = decimal.Parse(row.volume.ToString().Replace(",", string.Empty), CultureInfo.InvariantCulture)
                });
            }

            return result;
        }

        protected override async Task<List<QuoteModel>> GetQuotes(string url)
        {
            var data = string.Empty;

            WebClient wc = new();
            wc.Headers["cookie"] = "";

            using (var reader = new System.IO.StreamReader(wc.OpenRead(url) ?? throw new InvalidOperationException($"Не удалось получить данные по адресу '{url}'")))
            {
                data = await reader.ReadToEndAsync();
            }
            var response = ParseQuotes(data);
            return response;
        }
    }
}
