using CommonLibraries.Graal.Models;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetNearestQuoteResponse
    {
        public bool IsSuccess { get; set; }

        public QuoteModel Quote { get; set; }
    }
}
