using QuotesService.Api.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.ApiPrivate.Models.RequestResponse
{
    public class SetQuotesProviderParametersRequest
    {
        public QuotesProviderEnum QuotesProvider { get; set; }

        public string TickerName { get; set; }

        public string MarketName { get; set; }

        public List<KeyValuePair<string, string>> Parameters = new List<KeyValuePair<string, string>>();
    }
}
