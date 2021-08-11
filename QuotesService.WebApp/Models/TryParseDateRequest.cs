using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Models
{
    public class TryParseDateRequest
    {
        public string Format { get; set; }

        public string Text { get; set; }
    }
}
