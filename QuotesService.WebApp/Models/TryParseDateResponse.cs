using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Models
{
    public class TryParseDateResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int Hour { get; set; }

        public int Min { get; set; }

        public int Sec { get; set; }
    }
}
