using CommonLibraries.EF;
using CommonLibraries.Graal.Enums;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface IQuotesProvidersTasksRepository : IBaseRepository<QuotesProviderTaskEntity>
    {
        Task<Dictionary<TimeFrameEnum, QuotesProviderTaskEntity>> GetTasksByTickerId(int tickerId);

        Task<QuotesProviderTaskEntity> GetTaskById(int id);

        Task<List<QuotesProviderTaskEntity>> GetAllActiveTasks();

        Task<List<QuotesProviderTaskEntity>> GetAllActiveTasksWithFirstDate();

        Task<List<QuotesProviderTaskEntity>> GetAllActiveTasksWithoutFirstDate();

        Task<QuotesProviderTypeEnum> GetQuotesProviderType(int quotesProviderTaskId);

        Task<TimeFrameEnum> GetQuotesProviderTimeFrame(int quotesProviderTaskId);

        Task<TickerAndMarket> GetQuotesProviderTickerAndMarket(int quotesProviderTaskId);
    }
}
