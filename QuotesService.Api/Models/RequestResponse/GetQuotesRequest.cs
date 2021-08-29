using System;
using QuotesService.Api.Enum;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetQuotesRequest
    {
        public string TickerName { get; set; }

        public string MarketName { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate
        {
            get => _endDate ?? DateTime.Now;
            set => _endDate = value;
        }

        private DateTime? _endDate;
    }
}
