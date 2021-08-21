using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Core.Base;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetQuotesResponse
    {
        public List<QuoteModel> Quotes { get; set; } = new List<QuoteModel>();
    }
}
