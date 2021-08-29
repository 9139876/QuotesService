using CommonLibraries.EF;
using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface IQuotesRepository : IBaseRepository<QuoteEntity>
    {
        Task<List<QuoteEntity>> GetQuotes(GetQuotesRequest request);

        Task<QuoteEntity> GetLastQuote(GetSpecificQuoteRequest request);
    }
}
