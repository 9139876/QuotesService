using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Core.Extensions;

namespace QuotesService.Api.Enum
{
    public enum TickerTypeEnum
    {
        /// <summary>
        /// Акции
        /// </summary>
        [Description("Акции")]
        Stocks = 1,
        /// <summary>
        /// Индексы
        /// </summary>
        [Description("Индексы")]
        Indices = 2,
        /// <summary>
        /// Товары
        /// </summary>
        [Description("Товары")]
        Commodities = 3,
        /// <summary>
        /// Фьючерсы
        /// </summary>
        [Description("Фьючерсы")]
        Futures = 4,
        /// <summary>
        /// Опционы
        /// </summary>
        [Description("Опционы")]
        Options = 5,
        /// <summary>
        /// Другое
        /// </summary>
        [Description("Другое")]
        Other = 6
    }
}
