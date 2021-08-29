using System;
using QuotesService.Api.Enum;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetQuotesWithQPRequest : GetQuotesRequest
    {
        public QuotesProviderEnum QuotesProvider { get; set; }
    }
}
