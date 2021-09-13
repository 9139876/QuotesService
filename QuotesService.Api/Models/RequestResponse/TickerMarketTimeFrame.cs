using QuotesService.Api.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.Api.Models.RequestResponse
{
    public class TickerMarketTimeFrame
    {
        public string MarketName { get; set; }

        public string TickerName { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }
    }
}
