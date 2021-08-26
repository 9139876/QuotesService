using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.Api.Enum;
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
