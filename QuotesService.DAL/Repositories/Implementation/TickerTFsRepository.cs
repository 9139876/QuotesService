using CommonLibraries.EF.Implementation;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommonLibraries.Graal.Enums;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class TickerTFsRepository : BaseRepository<TickerTFEntity>, ITickerTFsRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public TickerTFsRepository(IQuotesDbContext dbcontext) : base(dbcontext.TickerTFs)
        {
            _dbcontext = dbcontext;
        }

        public async Task<TickerTFEntity> GetByTickerIdAndTF(int tickerId, TimeFrameEnum tf)
        {
            return await DbSet.SingleOrDefaultAsync(x => x.TickerId == tickerId && x.TimeFrame == tf);
        }
    }
}
