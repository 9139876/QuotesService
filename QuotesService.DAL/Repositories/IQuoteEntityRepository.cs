using CommonLibraries.EF;
using QuotesService.DAL.Entities;

namespace QuotesService.DAL.Repositories
{
    public interface IQuoteEntityRepository : IBaseRepository<QuoteEntity>
    {
    }
}
