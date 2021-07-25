using CommonLibraries.EF.Implementation;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class TickerInfoEntityRepository : BaseRepository<TickerInfoEntity>, ITickerInfoEntityRepository
    {
        private readonly IMarketsDbContext _dbcontext;

        public TickerInfoEntityRepository(IMarketsDbContext dbcontext) : base(dbcontext.TickerInfoEntity)
        {
            _dbcontext = dbcontext;
        }
    }
}
