using QuotesService.Api.Enum;

namespace QuotesService.Api.Models.RequestResponse
{
    public class TickerMarketTimeFrame
    {
        public string MarketName { get; set; }

        public string TickerName { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }
    }
}
