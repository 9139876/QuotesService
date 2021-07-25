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
        [Description("Акции")]
        Stocks = 1,
        [Description("Индексы")]
        Indices = 2,
        [Description("Товары")]
        Commodities = 3,
        [Description("Фьючерсы")]
        Futures = 4,
        [Description("Опционы")]
        Options = 5
    }
}
