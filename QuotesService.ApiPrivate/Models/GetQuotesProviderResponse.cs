using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.ApiPrivate.Models
{
    public class GetQuotesProviderResponse
    {
        public QuotesProvider CurrentQuotesProvider { get; set; }

        public List<QuotesProvider> AllQuotesProviders { get; set; } = new List<QuotesProvider>();
    }
}
