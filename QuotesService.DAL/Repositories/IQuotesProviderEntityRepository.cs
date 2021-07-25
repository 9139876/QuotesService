using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.EF;
using QuotesService.DAL.Entities;

namespace QuotesService.DAL.Repositories
{
    public interface IQuotesProviderEntityRepository : IBaseRepository<QuotesProviderEntity>
    {
        List<QuotesProviderEntity> GetAllQuotesProviders();

        Task<QuotesProviderEntity> GetQuotesProviderByName(string name);
    }
}
