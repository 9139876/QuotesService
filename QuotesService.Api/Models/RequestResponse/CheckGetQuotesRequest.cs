using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Core.Base;
using QuotesService.Api.Enum;

namespace QuotesService.Api.Models.RequestResponse
{
    public class CheckGetQuotesRequest : BaseGuidRequest
    {
        public QuotesProviderEnum QuotesProvider { get; set; }

        public string Symbol { get; set; }

        public DateTime StartDate { get; set; }
    }
}
