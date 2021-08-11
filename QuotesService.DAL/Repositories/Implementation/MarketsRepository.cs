using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.Api.Enum;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Models;


namespace QuotesService.DAL.Repositories.Implementation
{
    internal class MarketsRepository : BaseRepository<MarketEntity>, IMarketsRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public MarketsRepository(IQuotesDbContext dbcontext) : base(dbcontext.Markets)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<MarketEntity>> GetAllMarkets()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<MarketEntity> GetMarketByName(string name)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
