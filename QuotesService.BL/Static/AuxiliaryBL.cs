using CommonLibraries.Core.Extensions;
using CommonLibraries.Graal.Enums;
using CommonLibraries.Graal.Extensions;
using CommonLibraries.Graal.Models;
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
    internal static class AuxiliaryBL
    {
        /// <summary>
        /// Возвращает конечную дату для партии котировок для получения у поставщика котировок
        /// </summary>
        /// <param name="startDate">Начальная дата партии котировок </param>
        /// <param name="timeFrame">Таймфрейм</param>
        /// <returns></returns>
        public static DateTime GetEndBatchDate(DateTime startDate, TimeFrameEnum timeFrame)
        {
            var possibleEndDate = DateTimeExtensions.GetPossibleEndDate(timeFrame, DateTime.Now);

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
        /// Ищет дату первой имеющейся у поставщика котировки
        /// </summary>
        /// <param name="tickerMarketTimeFrame">Запрос</param>
        /// <param name="quotesProvider">Поставщик котировок</param>
        /// <param name="yearsAgo">На сколько лет назад смотреть</param>
        /// <returns></returns>
        //public async static Task<DateTime> SearchFirstDate(TickerMarketTimeFrame tickerMarketTimeFrame, Func<GetQuotesRequest, Task<List<QuoteModel>>> getQuotesDelegate, int yearsAgo = 30)
        //{
        //    var end = DateTime.Now;
        //    var start = end.AddYears(-yearsAgo);

        //    var getQuotesRequest = new GetQuotesRequest()
        //    {
        //        TickerMarketTimeFrame = tickerMarketTimeFrame
        //    };

        //    while ((end - start).TotalDays >= 1)
        //    {
        //        var median = GetMedian(start, end);
        //        getQuotesRequest.StartDate = median;
        //        getQuotesRequest.EndDate = GetEndBatchDate(getQuotesRequest.StartDate, getQuotesRequest.TickerMarketTimeFrame.TimeFrame);

        //        var quotes = await getQuotesDelegate(getQuotesRequest);// quotesProvider.GetQuotes(getQuotesRequest);
        //        quotes.RequiredNotNull(nameof(quotes), getQuotesRequest);

        //        if (quotes.Any())
        //        {
        //            var quotesMinDate = quotes.Min(x => x.Date);

        //            //Если дата первой полученной котировки существенно меньше запрошенной даты - считаем, что это и есть первая имеющаяся котировка
        //            if (IsEssentialDifferentDates(quotesMinDate, median, getQuotesRequest.TickerMarketTimeFrame.TimeFrame))
        //            {
        //                return quotesMinDate;
        //            }

        //            end = median;
        //        }
        //        else
        //        {
        //            start = median;
        //        }

        //        //Не DDOS`им поставщика котировок!!!
        //        Thread.Sleep(5000);
        //    }

        //    return start;
        //}

        /// <summary>
        /// Приводит котировки к единому формату по времени
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns></returns>
        public static List<QuoteModel> CorrectQuotes(QuotesCorrectRequest request)
        {
            request.Quotes.ForEach(x => x.Date = DateTimeExtensions.CorrectDateByTF(x.Date, request.TimeFrame));
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

        /// <summary>
        /// Признак, существенно ли первая дата больше второй для данного таймфрейма
        /// </summary>
        /// <param name="firstDate">Первая дата</param>
        /// <param name="secondDate">Вторая дата</param>
        /// <param name="timeFrame"></param>
        /// <returns></returns>
        private static bool IsEssentialDifferentDates(DateTime firstDate, DateTime secondDate, TimeFrameEnum timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrameEnum.tick:
                case TimeFrameEnum.min1:
                case TimeFrameEnum.min4:
                case TimeFrameEnum.H1:
                case TimeFrameEnum.D1:
                    {
                        return (firstDate - secondDate).TotalDays > 15; // длинные праздники
                    }

                case TimeFrameEnum.W1: return (firstDate - secondDate).TotalDays > 21; //3 недели
                case TimeFrameEnum.M1: return (firstDate - secondDate).TotalDays > 62; //2 месяца по 31 дню
                case TimeFrameEnum.Y1: return (firstDate - secondDate).TotalDays > 750; //больше 2 лет

                default: throw new InvalidOperationException($"Неизвестный таймфрейм - '{timeFrame.ToString()}'");
            };
        }



    }
}
