using QuotesService.Api.Enum;
using System;

namespace QuotesService.Api.Models.RequestResponse
{
    public class QuotesInfo
    {
        public string TimeFrameName { get; set; }

        public int QuotesCount { get; set; }

        public string FirstDate { get; set; }

        public string LastDate { get; set; }

        public string MinPrice { get; set; }

        public string MinPriceDate { get; set; }

        public string MaxPrice { get; set; }

        public string MaxPriceDate { get; set; }
    }
}
