using System.Collections.Generic;
using QuotesService.Api.Enum;

namespace QuotesService.BL.Models
{
    public class CheckGetQuotesRequest
    {
        public QuotesProviderEnum QuotesProviderType { get; set; }

        public List<KeyValuePair<string, string>> Parameters { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
