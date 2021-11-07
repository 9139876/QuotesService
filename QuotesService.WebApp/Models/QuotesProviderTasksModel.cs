using CommonLibraries.Core.Models;
using System.Collections.Generic;

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
