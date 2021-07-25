using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotesService.Api.Enum;

namespace QuotesService.Api.Models.RequestResponse
{
    public class AddMarketRequest
    {
        public QuotesProviderEnum QuotesProvider { get; set; }

        public string MarketName { get; set; }
    }
}
