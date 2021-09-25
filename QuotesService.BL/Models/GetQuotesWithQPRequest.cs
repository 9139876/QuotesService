using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;

namespace QuotesService.BL.Models
{
    public class GetQuotesWithQPRequest : GetQuotesRequest
    {
        public QuotesProviderEnum QuotesProvider { get; set; }
    }
}
