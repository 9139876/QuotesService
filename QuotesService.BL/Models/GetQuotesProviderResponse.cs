using System.Collections.Generic;

namespace QuotesService.BL.Models
{
    public class GetQuotesProviderResponse
    {
        public bool QuotesProviderAssigned { get; set; }

        public QuotesProvider CurrentQuotesProvider { get; set; }

        public List<QuotesProvider> AllQuotesProviders { get; set; } = new ();
    }
}
