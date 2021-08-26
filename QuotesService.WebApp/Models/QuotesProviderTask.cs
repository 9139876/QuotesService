using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Models
{
    public class QuotesProviderTask
    {
        public string TimeFrameName { get; set; }

        public bool IsActive { get; set; }

        public int UpdatePeriodInSecond { get; set; }

        public string LastUpdateDate { get; set; }
    }
}
