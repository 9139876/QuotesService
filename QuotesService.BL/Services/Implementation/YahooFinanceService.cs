using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;

namespace QuotesService.BL.Services.Implementation
{
    internal class YahooFinanceService : IYahooFinanceService
    {
        public async Task<TryGetTickerInfoYFFromServerResponse> TryGetTickerInfoFromServerAsync(TryGetTickerInfoYFFromServerRequest request)
        {
            var response = new TryGetTickerInfoYFFromServerResponse() { GUID = request.GUID };

            var url = $"https://finance.yahoo.com/quote/{request.Code.ToUpper()}/profile?p={request.Code.ToUpper()}";

            var webClient = new WebClient();
            System.IO.Stream stream = webClient.OpenRead(url);
            using (var reader = new System.IO.StreamReader(stream))
            {
                response.Data = await reader.ReadToEndAsync();
            }

            return response;
        }

        public async Task<GetQuotesResponse> GetQuotes(GetQuotesRequest request)
        {
            var response = new GetQuotesResponse() { GUID = request.GUID };

            var url = GetQuotesURL(request.Symbol, request.StartDate, request.EndDate, request.TimeFrame);

            using (var reader = new System.IO.StreamReader(new WebClient().OpenRead(url) ?? throw new InvalidOperationException($"Не удалось получить данные по адресу '{url}'")))
            {
                var data = await reader.ReadToEndAsync();
                response.Quotes = ParseQuotes(data);
            }

            return response;
        }

        public async Task<CheckGetQuotesResponse> CheckGetQuotes(CheckGetQuotesRequest request)
        {
            var getQuotesRequest = new GetQuotesRequest()
            {
                Symbol = request.Symbol,
                StartDate = request.StartDate,
                EndDate = request.StartDate.AddMonths(1),
                TimeFrame = TimeFrameEnum.D1
            };

            try
            {
                var quotes = await GetQuotes(getQuotesRequest);

                if (quotes.Quotes.Any())
                {
                    return new CheckGetQuotesResponse()
                    {
                        GUID = request.GUID,
                        IsSuccess = true,
                        StartDate = quotes.Quotes.Select(x => x.Date).Min()
                    };
                }
                else
                {
                    return new CheckGetQuotesResponse()
                    {
                        GUID = request.GUID,
                        IsSuccess = false,
                        ErrorMessage = $"Для начальной даты {request.StartDate.ToString("d")} в течение месяча не было ни одной котировки"
                    };
                }
            }
            catch (Exception e)
            {
                return new CheckGetQuotesResponse()
                {
                    GUID = request.GUID,
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        #region private static
        private static readonly DateTime ZeroDt = new DateTime(1970, 1, 1);

        private static string GetQuotesURL(string tickerSymbol, DateTime start, DateTime end, TimeFrameEnum timeFrame)
        {
            start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            end = new DateTime(end.Year, end.Month, end.Day, 20, 0, 0);

            return $"https://query1.finance.yahoo.com/v7/finance/download/{tickerSymbol}?period1={GetDateValue(start)}&period2={GetDateValue(end)}&interval={GetTimeFrameCode(timeFrame)}&events=history&includeAdjustedClose=true";
        }

        private static long GetDateValue(DateTime dt)
        {
            return (long)((dt - ZeroDt).TotalSeconds);
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

        private List<QuoteModel> ParseQuotes(string data)
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
        #endregion
    }
}
