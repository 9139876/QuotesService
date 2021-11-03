using CommonLibraries.EF;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface IQuotesRepository : IBaseRepository<QuoteEntity>
    {
        Task<List<QuoteEntity>> GetQuotes(GetQuotesRequest request);

        Task<QuoteEntity> GetLastQuote(TickerMarketTimeFrame request);

        Task<List<QuoteEntity>> GetAllQuotes(TickerMarketTimeFrame request);

        Task<QuoteEntity> GetNearestQuote(GetNearestQuoteRequest request);
    }
}
