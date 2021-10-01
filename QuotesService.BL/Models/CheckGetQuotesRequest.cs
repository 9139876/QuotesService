using System.Collections.Generic;
using QuotesService.Api.Enum;

namespace QuotesService.BL.Models
{
    public class CheckGetQuotesRequest
    {
        public QuotesProviderTypeEnum QuotesProviderType { get; set; }

        public List<KeyValuePair<string, string>> Parameters { get; set; } = new ();
    }
}
