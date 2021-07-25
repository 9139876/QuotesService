using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.Api.Enum;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class MarketEntityRepository : BaseRepository<MarketEntity>, IMarketEntityRepository
    {
        private readonly IMarketsDbContext _dbcontext;

        public MarketEntityRepository(IMarketsDbContext dbcontext) : base(dbcontext.MarketsEntities)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<MarketEntity>> GetQuotesProviderMarkets(QuotesProviderEnum quotesProvider)
        {
            return await DbSet.Where(x => x.QuotesProvider == quotesProvider).ToListAsync();
        }

        public async Task<MarketEntity> GetByQuotesProviderAndName(QuotesProviderEnum quotesProvider, string name)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.QuotesProvider == quotesProvider && x.Name == name);
        }
    }
}
