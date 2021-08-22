using System.Collections.Generic;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetQuotesResponse
    {
        public List<QuoteModel> Quotes { get; set; } = new List<QuoteModel>();
    }
}
