using CommonLibraries.EF.Implementation;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class QuoteEntityRepository : BaseRepository<QuoteEntity>, IQuoteEntityRepository
    {
        private readonly IMarketsDbContext _dbcontext;

        public QuoteEntityRepository(IMarketsDbContext dbcontext) : base(dbcontext.QuoteEntity)
        {
            _dbcontext = dbcontext;
        }
    }
}
