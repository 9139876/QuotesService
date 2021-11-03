using CommonLibraries.EF.Implementation;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using QuotesService.Api.Models;

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

        public async Task<List<TickerAndMarket>> GetAllTickersAndMarkets(GetAllTickersAndMarketsRequest request)
        {
            var query = from tickers in _dbcontext.Tickers
                        join markets in _dbcontext.Markets on tickers.MarketId equals markets.Id
                        select new { tickers, markets };

            if (request.AllowedMarketsNames?.Any() == true)
            {
                query = query.Where(x => request.AllowedMarketsNames.Contains(x.markets.Name));
            }

            if (request.AllowedTickersNames?.Any() == true)
            {
                query = query.Where(x => request.AllowedTickersNames.Contains(x.tickers.Name));
            }

            if (request.AllowedQuotesProviderTypes?.Any() == true)
            {
                query = query.Where(x => x.tickers.QuotesProviderType.HasValue && request.AllowedQuotesProviderTypes.Contains(x.tickers.QuotesProviderType.Value));
            }

            return (await query.ToListAsync())
                .Select(x => new TickerAndMarket()
                {
                    MarketName = x.markets.Name,
                    TickerName = x.tickers.Name
                }).ToList();
        }

        public async Task<TickerEntity> GetByTickerAndMarket(TickerAndMarket request)
        {
            var query = from tickers in _dbcontext.Tickers
                        join markets in _dbcontext.Markets on tickers.MarketId equals markets.Id
                        where tickers.Name == request.TickerName && markets.Name == request.MarketName
                        select tickers;

            return await query.SingleOrDefaultAsync();
        }
    }
}
