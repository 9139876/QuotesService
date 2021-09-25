using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using System.Collections.Generic;

namespace QuotesService.BL.Models
{
    public class QuotesToStorageRequest
    {
        public List<QuoteModel> Quotes { get; set; } = new();

        public int TickerTfId { get; set; }

        public TickerMarketTimeFrame TickerMarketTimeFrame { get; set; }

        public bool IsUpdateDifferenceQuotes { get; set; }
    }
}
