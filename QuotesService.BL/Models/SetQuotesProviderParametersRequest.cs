using QuotesService.Api.Enum;
using System.Collections.Generic;

namespace QuotesService.BL.Models
{
    public class SetQuotesProviderParametersRequest
    {
        public QuotesProviderEnum QuotesProviderType { get; set; }

        public string TickerName { get; set; }

        public string MarketName { get; set; }

        public List<KeyValuePair<string, string>> Parameters = new List<KeyValuePair<string, string>>();
    }
}
