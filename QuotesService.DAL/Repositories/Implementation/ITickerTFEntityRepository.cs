using CommonLibraries.EF.Implementation;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class TickerTFEntityRepository : BaseRepository<TickersTimeFramesEntity>, ITickerTFEntityRepository
    {
        private readonly IMarketsDbContext _dbcontext;

        public TickerTFEntityRepository(IMarketsDbContext dbcontext) : base(dbcontext.TickerTFEntity)
        {
            _dbcontext = dbcontext;
        }
    }
}
