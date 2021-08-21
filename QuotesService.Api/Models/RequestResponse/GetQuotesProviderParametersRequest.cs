using QuotesService.Api.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetQuotesProviderParametersRequest
    {
        public QuotesProviderEnum QuotesProvider { get; set; }

        public string TickerName { get; set; }

        public string MarketName { get; set; }
    }
}
