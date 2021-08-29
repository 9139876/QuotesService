using CommonLibraries.EF.Implementation;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Models;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class QuotesRepository : BaseRepository<QuoteEntity>, IQuotesRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public QuotesRepository(IQuotesDbContext dbcontext) : base(dbcontext.Quotes)
        {
            _dbcontext = dbcontext;
        }

        public async Task<QuoteEntity> GetLastQuote(GetSpecificQuoteRequest request)
        {
            var query = from q in _dbcontext.Quotes
                        join ttf in _dbcontext.TickerTFs on q.ParentTickerTFId equals ttf.Id
                        join t in _dbcontext.Tickers on ttf.TickerId equals t.Id
                        join m in _dbcontext.Markets on t.MarketId equals m.Id
                        where ttf.TimeFrame == request.TimeFrame
                            && t.Name == request.TickerName
                            && m.Name == request.MarketName
                        select q;

            return await query.OrderByDescending(x => x.Date).LastOrDefaultAsync();
        }

        public async Task<List<QuoteEntity>> GetQuotes(GetQuotesRequest request)
        {
            var query = from q in _dbcontext.Quotes
                        join ttf in _dbcontext.TickerTFs on q.ParentTickerTFId equals ttf.Id
                        join t in _dbcontext.Tickers on ttf.TickerId equals t.Id
                        join m in _dbcontext.Markets on t.MarketId equals m.Id
                        where m.Name == request.MarketName
                            && t.Name == request.TickerName
                            && ttf.TimeFrame == request.TimeFrame
                            && q.Date >= request.StartDate
                            && q.Date <= request.EndDate
                        select q;

            return await query.ToListAsync();
        }
    }
}
