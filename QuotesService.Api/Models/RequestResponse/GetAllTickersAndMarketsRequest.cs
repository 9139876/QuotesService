using QuotesService.Api.Enum;
using System.Collections.Generic;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetAllTickersAndMarketsRequest
    {
        public List<QuotesProviderTypeEnum> AllowedQuotesProviderTypes { get; set; } = new();

        public List<string> AllowedMarketsNames { get; set; }

        public List<string> AllowedTickersNames { get; set; }
    }
}
