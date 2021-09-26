using System;
using QuotesService.Api.Enum;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetQuotesRequest
    {
        public TickerMarketTimeFrame TickerMarketTimeFrame { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
