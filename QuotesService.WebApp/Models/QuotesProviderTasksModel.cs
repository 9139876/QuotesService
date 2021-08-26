using QuotesService.Api.Models.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Models
{
    public class QuotesProviderTasksModel
    {
        public StandartResponse Status { get; set; }

        public string MarketName { get; set; }

        public string TickerName { get; set; }

        public List<QuotesProviderTask> QuotesProviderTasks { get; set; }
    }
}
