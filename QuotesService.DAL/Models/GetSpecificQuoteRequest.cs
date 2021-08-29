using QuotesService.Api.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Models
{
    public class GetSpecificQuoteRequest
    {
        public string TickerName { get; set; }

        public string MarketName { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }
    }
}
