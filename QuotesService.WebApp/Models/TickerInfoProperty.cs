using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Models
{
    public class TickerInfoProperty
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool ReadOnly { get; set; }
    }
}
