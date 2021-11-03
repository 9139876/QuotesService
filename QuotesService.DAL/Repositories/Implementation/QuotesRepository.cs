using CommonLibraries.EF.Implementation;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Models;
using CommonLibraries.Graal.Extensions;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class QuotesRepository : BaseRepository<QuoteEntity>, IQuotesRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public QuotesRepository(IQuotesDbContext dbcontext) : base(dbcontext.Quotes)
        {
            _dbcontext = dbcontext;
        }

        public async Task<QuoteEntity> GetLastQuote(TickerMarketTimeFrame request)
        {
            var query = from q in _dbcontext.Quotes
                        join ttf in _dbcontext.TickerTFs on q.ParentTickerTFId equals ttf.Id
                        join t in _dbcontext.Tickers on ttf.TickerId equals t.Id
                        join m in _dbcontext.Markets on t.MarketId equals m.Id
                        where ttf.TimeFrame == request.TimeFrame
                            && t.Name == request.TickerName
                            && m.Name == request.MarketName
                        select q;

            return await query.OrderBy(x => x.Date).LastOrDefaultAsync();
        }

        public async Task<List<QuoteEntity>> GetQuotes(GetQuotesRequest request)
        {
            var query = from q in _dbcontext.Quotes
                        join ttf in _dbcontext.TickerTFs on q.ParentTickerTFId equals ttf.Id
                        join t in _dbcontext.Tickers on ttf.TickerId equals t.Id
                        join m in _dbcontext.Markets on t.MarketId equals m.Id
                        where m.Name == request.TickerMarketTimeFrame.MarketName
                            && t.Name == request.TickerMarketTimeFrame.TickerName
                            && ttf.TimeFrame == request.TickerMarketTimeFrame.TimeFrame
                            && q.Date >= request.StartDate
                            && q.Date <= request.EndDate
                        select q;

            return await query.ToListAsync();
        }

        public async Task<List<QuoteEntity>> GetAllQuotes(TickerMarketTimeFrame request)
        {
            var query = from q in _dbcontext.Quotes
                        join ttf in _dbcontext.TickerTFs on q.ParentTickerTFId equals ttf.Id
                        join t in _dbcontext.Tickers on ttf.TickerId equals t.Id
                        join m in _dbcontext.Markets on t.MarketId equals m.Id
                        where m.Name == request.MarketName
                           && t.Name == request.TickerName
                           && ttf.TimeFrame == request.TimeFrame
                        select q;

            return await query.ToListAsync();
        }

        public async Task<QuoteEntity> GetNearestQuote(GetNearestQuoteRequest request)
        {
            var dt1 = DateTimeExtensions.CorrectDateByTF(request.Date, request.TickerMarketTimeFrame.TimeFrame);

            var query = from q in _dbcontext.Quotes
                        join ttf in _dbcontext.TickerTFs on q.ParentTickerTFId equals ttf.Id
                        join t in _dbcontext.Tickers on ttf.TickerId equals t.Id
                        join m in _dbcontext.Markets on t.MarketId equals m.Id
                        where ttf.TimeFrame == request.TickerMarketTimeFrame.TimeFrame
                            && t.Name == request.TickerMarketTimeFrame.TickerName
                            && m.Name == request.TickerMarketTimeFrame.MarketName
                        select q;

            var exact = await query.Where(x => x.Date == dt1).FirstOrDefaultAsync();

            if (exact != null)
            {
                return exact;
            }
            else
            {
                var dt2 = dt1.AddDays(-15);
                var nearest = await query.Where(x => x.Date <= dt1 && x.Date >= dt2).OrderBy(x => x.Date).LastOrDefaultAsync();

                return nearest;
            }
        }
    }
}
