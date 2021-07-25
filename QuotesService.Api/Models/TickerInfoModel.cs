namespace QuotesService.Api.Models
{
    public class TickerInfoModel
    {
        public string TickerId { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string MarketId { get; set; }

        public string MarketTitle { get; set; }

        public string Currency { get; set; }

        public string VolumeCode { get; set; }
    }
}
