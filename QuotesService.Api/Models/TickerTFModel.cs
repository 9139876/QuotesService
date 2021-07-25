using QuotesService.Api.Enum;

namespace QuotesService.Api.Models
{
    public class TickerTFModel
    {
        public TimeFrameEnum TimeFrame { get; set; }

        public TickerInfoModel ParentTickerInfo { get; set; }

        //public List<QuoteModel> Quotes { get; set; }

        //public DateTime QuotesDateOfLastUpdate { get; set; }
    }
}
