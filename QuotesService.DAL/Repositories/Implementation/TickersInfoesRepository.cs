using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class TickersInfoesRepository : BaseRepository<TickerInfoEntity>, ITickersInfoesRepository
    {
        private readonly IQuotesDbContext _dbContext;

        public TickersInfoesRepository(IQuotesDbContext dbContext) : base(dbContext.TickersInfoes)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TickerInfoEntity>> GetTickerInfoesByTickerId(int tickerId)
        {
            return await DbSet.Where(x => x.TickerId == tickerId).ToListAsync();
        }
    }
}
