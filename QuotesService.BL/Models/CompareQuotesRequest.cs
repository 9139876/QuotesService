using QuotesService.Api.Enum;
using QuotesService.Api.Models;

namespace QuotesService.BL.Models
{
    public class CompareQuotesRequest
    {
        public TickerAndMarket TickerAndMarketFirst { get; set; }

        public TickerAndMarket TickerAndMarketSecond { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }

        //public bool CheckSkips { get; set; }

        public int MaxDifferencePercent { get; set; }
    }
}
