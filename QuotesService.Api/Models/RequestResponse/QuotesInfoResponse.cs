using System;

namespace QuotesService.Api.Models.RequestResponse
{
    public class QuotesInfoResponse
    {
        public int QuotesCount { get; set; }

        public DateTime FirstDate { get; set; }

        public DateTime LastDate { get; set; }

        public decimal MinPrice { get; set; }

        public DateTime MinPriceDate { get; set; }

        public decimal MaxPrice { get; set; }

        public DateTime MaxPriceDate { get; set; }
    }
}
