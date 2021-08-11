using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.Api.Models.RequestResponse
{
    public class TickerAndMarketRequest
    {
        public string MarketName { get; set; }

        public string TickerName { get; set; }
    }
}
