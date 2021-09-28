using System.Collections.Generic;

namespace QuotesService.BL.Models
{
    public class CompareQuotesResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string FirstComparedDate { get; set; }

        public string LastComparedDate { get; set; }

        public List<string> OnlyInFirstTicker { get; set; } = new();

        public List<string> OnlyInSecondTicker { get; set; } = new();

        public List<string> Differences { get; set; } = new();
    }
}
