using CommonLibraries.EF.Implementation;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class QuotesRepository : BaseRepository<QuoteEntity>, IQuotesRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public QuotesRepository(IQuotesDbContext dbcontext) : base(dbcontext.Quotes)
        {
            _dbcontext = dbcontext;
        }
    }
}
