using CommonLibraries.BackgroundWorker;
using CommonLibraries.BackgroundWorker.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuotesService.WebScheduler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebScheduler.BackgroundWorkers
{
    public class GetQuotesBackgroundWorker : ListBackgroundWorker<int, IGetQuotesBackgroundService>
    {
        public GetQuotesBackgroundWorker(
            ILogger<GetQuotesBackgroundWorker> logger,
            IServiceProvider serviceProvider,
            IOptions<BackgroundWorkersOptions> optionsAccessor) : base(logger, serviceProvider, optionsAccessor) { }
    }
}
