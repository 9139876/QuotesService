using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using System.Collections.Generic;

namespace QuotesService.BL.Models
{
    public class QuotesCorrectRequest
    {
        public List<QuoteModel> Quotes { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }
    }
}
