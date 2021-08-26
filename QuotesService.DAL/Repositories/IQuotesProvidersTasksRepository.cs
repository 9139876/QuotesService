using CommonLibraries.EF;
using QuotesService.Api.Enum;
using QuotesService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface IQuotesProvidersTasksRepository : IBaseRepository<QuotesProviderTaskEntity>
    {
        Task<Dictionary<TimeFrameEnum, QuotesProviderTaskEntity>> GetTasksByTickerId(int tickrId);
    }
}
