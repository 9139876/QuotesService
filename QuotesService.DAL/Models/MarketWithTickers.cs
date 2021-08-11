using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Models
{
    public class MarketWithTickers
    {
        public string MarketName { get; set; }

        public List<string> TickersNames { get; set; } = new List<string>();
    }
}
