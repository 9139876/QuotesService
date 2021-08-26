using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
