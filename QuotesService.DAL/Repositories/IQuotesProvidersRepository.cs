using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.EF;
using QuotesService.Api.Enum;
using QuotesService.DAL.Entities;

namespace QuotesService.DAL.Repositories
{
    public interface IQuotesProvidersRepository : IBaseRepository<QuotesProviderEntity>
    {
        Task<List<QuotesProviderEntity>> GetAllQuotesProviders();

        Task<QuotesProviderEntity> GetQuotesProviderByName(string name);

        Task<QuotesProviderEntity> GetQuotesProviderByType(QuotesProviderEnum quotesProvider);

        Task<QuotesProviderEntity> GetQuotesProviderById(int id);
    }
}
