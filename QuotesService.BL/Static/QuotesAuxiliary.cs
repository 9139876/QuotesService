using CommonLibraries.Core.Extensions;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuotesService.BL.Static
{
    public static class QuotesAuxiliary
    {
        public static DateTime GetEndBatchDate(DateTime startDate, TimeFrameEnum timeFrame)
        {
            var possibleEndDate = GetPossibleEndDate(timeFrame);

            switch (timeFrame)
            {
                case TimeFrameEnum.tick: return new DateTime(Math.Min(startDate.AddDays(15).Ticks, possibleEndDate.Ticks)); //меньше 15 дней может застрять на новогодних праздниках
                case TimeFrameEnum.min1: return new DateTime(Math.Min(startDate.AddDays(15).Ticks, possibleEndDate.Ticks));
                case TimeFrameEnum.min4: return new DateTime(Math.Min(startDate.AddDays(15).Ticks, possibleEndDate.Ticks));
                case TimeFrameEnum.H1: return new DateTime(Math.Min(startDate.AddMonths(1).Ticks, possibleEndDate.Ticks));
                case TimeFrameEnum.D1: return new DateTime(Math.Min(startDate.AddYears(1).Ticks, possibleEndDate.Ticks));
                case TimeFrameEnum.W1: return new DateTime(Math.Min(startDate.AddYears(3).Ticks, possibleEndDate.Ticks));
                case TimeFrameEnum.M1: return new DateTime(Math.Min(startDate.AddYears(10).Ticks, possibleEndDate.Ticks));
                case TimeFrameEnum.Y1: return new DateTime(Math.Min(startDate.AddYears(30).Ticks, possibleEndDate.Ticks));

                default: throw new InvalidOperationException($"Неизвестный таймфрейм - '{timeFrame.ToString()}'");
            };
        }

        /// <summary>
        /// Последняя возможно существующая дата котировки на текущий момент
        /// </summary>
        /// <param name="timeFrame"></param>
        /// <returns></returns>
        public static DateTime GetPossibleEndDate(TimeFrameEnum timeFrame)
        {
            var now = DateTime.Now;

            switch (timeFrame)
            {
                case TimeFrameEnum.tick: return now;
                case TimeFrameEnum.min1: return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 59).AddMinutes(-1);
                case TimeFrameEnum.min4: throw new NotImplementedException();//  return now;
                case TimeFrameEnum.H1: return new DateTime(now.Year, now.Month, now.Day, now.Hour, 59,59).AddHours(-1);
                case TimeFrameEnum.D1: return new DateTime(now.Year, now.Month, now.Day, 23, 59, 59).AddDays(-1);
                case TimeFrameEnum.W1: throw new NotImplementedException();//  return new DateTime(now.Year, now.Month, now.Day, now.Hour, 59, 59).AddHours(-1);
                case TimeFrameEnum.M1: return new DateTime(now.Year, now.Month, 28, 23, 59, 59).AddMonths(-1);
                case TimeFrameEnum.Y1: return new DateTime(now.Year, 12, 31, 23, 59, 59).AddYears(-1);

                default: throw new InvalidOperationException($"Неизвестный таймфрейм - '{timeFrame.ToString()}'");
            };
        }

        public async static Task<DateTime> SearchFirstDate(GetBatchQuotesRequest getLastBatchQuotesRequest, IQuotesProvider quotesProvider, int yearsAgo = 30)
        {
            var end = DateTime.Now;
            var start = end.AddYears(-yearsAgo);

            var getQuotesRequest = new GetQuotesRequest()
            {
                MarketName = getLastBatchQuotesRequest.MarketName,
                TickerName = getLastBatchQuotesRequest.TickerName,
                TimeFrame = getLastBatchQuotesRequest.TimeFrame
            };

            while ((end - start).TotalDays >= 1)
            {
                var median = GetMedian(start, end);
                getQuotesRequest.StartDate = median;
                getQuotesRequest.EndDate = GetEndBatchDate(getQuotesRequest.StartDate, getQuotesRequest.TimeFrame);

                var quotes = await quotesProvider.GetQuotes(getQuotesRequest);
                quotes.RequiredNotNull(nameof(quotes), getQuotesRequest);

                if (quotes.Quotes.Any())
                {
                    var quotesMinDate = quotes.Quotes.Min(x => x.Date);

                    //Если дата первой полученной котировки существенно меньше запрошенной даты - считаем, что это и есть первая имеющаяся котировка
                    if (IsEssentialDifferentDates(quotesMinDate, median, getQuotesRequest.TimeFrame))
                    {
                        return quotesMinDate;
                    }

                    end = median;
                }
                else
                {
                    start = median;
                }

                //Не DDOS`им поставщика котировок!!!
                Thread.Sleep(5000);
            }

            return start;
        }

        public static List<QuoteModel> CorrectQuotes(QuotesCorrectRequest request)
        {
            //Пока не реализовано
            return request.Quotes;
        }

        /// <summary>
        /// Проверяет, является ли дата котировки последней на текущий момент
        /// </summary>
        /// <param name="quoteDate"></param>
        /// <param name="timeFrame"></param>
        /// <returns></returns>
        public static bool IsReallyLastQuote(DateTime quoteDate, TimeFrameEnum timeFrame)
        {
            return !IsEssentialDifferentDates(DateTime.Now, quoteDate, timeFrame);
        }

        private static DateTime GetMedian(DateTime dt1, DateTime dt2)
        {
            if (dt1 == dt2)
            {
                return dt1;
            }

            var min = new DateTime(Math.Min(dt1.Ticks, dt2.Ticks));
            var max = new DateTime(Math.Max(dt1.Ticks, dt2.Ticks));

            return new DateTime(min.Ticks + (max.Ticks - min.Ticks) / 2);
        }

        private static bool IsEssentialDifferentDates(DateTime quotesMinDate, DateTime requestedMinDate, TimeFrameEnum timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrameEnum.tick:
                case TimeFrameEnum.min1:
                case TimeFrameEnum.min4:
                case TimeFrameEnum.H1:
                case TimeFrameEnum.D1:
                    {
                        return (quotesMinDate - requestedMinDate).TotalDays > 15; // длинные праздники
                    }

                case TimeFrameEnum.W1: return (quotesMinDate - requestedMinDate).TotalDays > 21; //3 недели
                case TimeFrameEnum.M1: return (quotesMinDate - requestedMinDate).TotalDays > 62; //2 месяца по 31 дню
                case TimeFrameEnum.Y1: return (quotesMinDate - requestedMinDate).TotalDays > 750; //больше 2 лет

                default: throw new InvalidOperationException($"Неизвестный таймфрейм - '{timeFrame.ToString()}'");
            };
        }
    }
}
