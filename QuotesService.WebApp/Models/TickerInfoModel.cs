using CommonLibraries.Core.Models;
using System.Collections.Generic;

namespace QuotesService.WebApp.Models
{
    public class TickerInfoModel
    {
        public StandartResponse Status { get; set; }

        public string TickerName { get; set; }

        public string MarketName { get; set; }

        public List<TickerInfoProperty> Properties { get; set; } = new List<TickerInfoProperty>();
    }
}
