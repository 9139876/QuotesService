using CommonLibraries.BackgroundWorker.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebScheduler.Services
{
    public interface IGetQuotesBackgroundService : IListBackgroundService<int>
    {
    }
}
