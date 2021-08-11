using CommonLibraries.EF.Implementation;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class TickerTFsRepository : BaseRepository<TickerTFEntity>, ITickerTFsRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public TickerTFsRepository(IQuotesDbContext dbcontext) : base(dbcontext.TickerTFs)
        {
            _dbcontext = dbcontext;
        }
    }
}
