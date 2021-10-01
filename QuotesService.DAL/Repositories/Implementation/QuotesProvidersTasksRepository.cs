using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class QuotesProvidersTasksRepository : BaseRepository<QuotesProviderTaskEntity>, IQuotesProvidersTasksRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public QuotesProvidersTasksRepository(IQuotesDbContext dbcontext) : base(dbcontext.QuotesProvidersTasks)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<QuotesProviderTaskEntity>> GetAllActiveTasks()
        {
            return await DbSet.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<List<QuotesProviderTaskEntity>> GetAllActiveTasksWithFirstDate()
        {
            var query = from qpt in _dbcontext.QuotesProvidersTasks
                        join ttf in _dbcontext.TickerTFs on qpt.TickerTFId equals ttf.Id
                        join q in _dbcontext.Quotes on ttf.Id equals q.ParentTickerTFId
                        where qpt.IsActive == true
                        select qpt;

            var queryResult = await query.Distinct().ToListAsync();

            return queryResult;
        }

        public async Task<List<QuotesProviderTaskEntity>> GetAllActiveTasksWithoutFirstDate()
        {
            var query = from qpt in _dbcontext.QuotesProvidersTasks
                        join ttf in _dbcontext.TickerTFs on qpt.TickerTFId equals ttf.Id
                        join q in _dbcontext.Quotes on ttf.Id equals q.ParentTickerTFId into all
                        from without in all.DefaultIfEmpty()
                        where qpt.IsActive == true
                            && without == null
                        select qpt;

            var queryResult = await query.Distinct().ToListAsync();

            return queryResult;
        }

        public async Task<TickerAndMarket> GetQuotesProviderTickerAndMarket(int quotesProviderTaskId)
        {
            var query = from qpt in _dbcontext.QuotesProvidersTasks
                        join ttf in _dbcontext.TickerTFs on qpt.TickerTFId equals ttf.Id
                        join t in _dbcontext.Tickers on ttf.TickerId equals t.Id
                        join m in _dbcontext.Markets on t.MarketId equals m.Id
                        where qpt.Id == quotesProviderTaskId
                        select new TickerAndMarket() { MarketName = m.Name, TickerName = t.Name };

            var queryResult = await query.SingleAsync();

            return queryResult;
        }

        public async Task<TimeFrameEnum> GetQuotesProviderTimeFrame(int quotesProviderTaskId)
        {
            var query = from qpt in _dbcontext.QuotesProvidersTasks
                        join ttf in _dbcontext.TickerTFs on qpt.TickerTFId equals ttf.Id
                        where qpt.Id == quotesProviderTaskId
                        select ttf.TimeFrame;

            var queryResult = await query.SingleAsync();

            return queryResult;
        }

        public async Task<QuotesProviderTypeEnum> GetQuotesProviderType(int quotesProviderTaskId)
        {
            var query = from qpt in _dbcontext.QuotesProvidersTasks
                        join ttf in _dbcontext.TickerTFs on qpt.TickerTFId equals ttf.Id
                        join t in _dbcontext.Tickers on ttf.TickerId equals t.Id
                        where qpt.Id == quotesProviderTaskId
                        select t.QuotesProviderType;

            var queryResult = (await query.SingleAsync()).ToString();

            if(!Enum.TryParse(queryResult, out QuotesProviderTypeEnum result))
            {
                throw new InvalidCastException($"QuotesProviderType by number value = {queryResult}");
            }

            return result;
        }

        public async Task<QuotesProviderTaskEntity> GetTaskById(int id)
        {
            return await DbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Dictionary<TimeFrameEnum, QuotesProviderTaskEntity>> GetTasksByTickerId(int tickerId)
        {
            var query = from qpt in _dbcontext.QuotesProvidersTasks
                        join ttf in _dbcontext.TickerTFs on qpt.TickerTFId equals ttf.Id
                        where ttf.TickerId == tickerId
                        select new { ttf.TimeFrame, qpt };

            var queryResult = await query.ToListAsync();

            return queryResult.ToDictionary(x => x.TimeFrame, x => x.qpt);
        }
    }
}
