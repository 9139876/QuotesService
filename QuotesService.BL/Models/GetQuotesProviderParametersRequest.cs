using QuotesService.Api.Enum;

namespace QuotesService.BL.Models
{
    public class GetQuotesProviderParametersRequest
    {
        public QuotesProviderTypeEnum QuotesProviderType { get; set; }

        public string TickerName { get; set; }

        public string MarketName { get; set; }
    }
}
