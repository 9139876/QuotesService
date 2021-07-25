using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.EF;
using QuotesService.Api.Enum;
using QuotesService.DAL.Entities;

namespace QuotesService.DAL.Repositories
{
    public interface IMarketEntityRepository : IBaseRepository<MarketEntity>
    {
        Task<List<MarketEntity>> GetQuotesProviderMarkets(QuotesProviderEnum quotesProvider);

        Task<MarketEntity> GetByQuotesProviderAndName(QuotesProviderEnum quotesProvider, string name);
    }
}
