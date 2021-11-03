using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetNearestQuoteRequest
    {
        public TickerMarketTimeFrame TickerMarketTimeFrame { get; set; }

        public DateTime Date { get; set; }
    }
}
