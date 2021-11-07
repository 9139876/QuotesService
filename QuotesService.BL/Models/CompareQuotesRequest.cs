using CommonLibraries.Graal.Enums;
using CommonLibraries.Graal.Models;

namespace QuotesService.BL.Models
{
    public class CompareQuotesRequest
    {
        public TickerAndMarket TickerAndMarketFirst { get; set; }

        public TickerAndMarket TickerAndMarketSecond { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }

        public int MaxDifferencePercent { get; set; }
    }
}
