using QuotesService.Api.Enum;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetBatchQuotesRequest
    {
        public string TickerName { get; set; }

        public string MarketName { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }
    }
}
