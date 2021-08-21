using CommonLibraries.EF;
using QuotesService.Api.Enum;
using QuotesService.DAL.Entities;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface IQuotesRepository : IBaseRepository<QuoteEntity>
    {
    }
}
