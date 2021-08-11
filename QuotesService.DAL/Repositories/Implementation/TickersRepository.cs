using CommonLibraries.EF.Implementation;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class TickersRepository : BaseRepository<TickerEntity>, ITickersRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public TickersRepository(IQuotesDbContext dbcontext) : base(dbcontext.Tickers)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<TickerEntity>> GetAllTickers()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TickerEntity> GetByTickerAndMarket(TickerAndMarketRequest request)
        {
            var query = from tickers in _dbcontext.Tickers
                        join markets in _dbcontext.Markets on tickers.MarketId equals markets.Id
                        where tickers.Name == request.TickerName && markets.Name == request.MarketName
                        select tickers;

            return await query.SingleOrDefaultAsync();
        }
    }
}
