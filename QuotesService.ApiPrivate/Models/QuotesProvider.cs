using QuotesService.Api.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.ApiPrivate.Models
{
    public class QuotesProvider
    {
        public QuotesProviderEnum QuotesProviderType { get; set; }

        public string QuotesProviderName { get; set; }
    }
}
