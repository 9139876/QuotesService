using CommonLibraries.EF;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface ITickersRepository : IBaseRepository<TickerEntity>
    {
        Task<TickerEntity> GetByTickerAndMarket(TickerAndMarketRequest request);

        Task<List<TickerEntity>> GetAllTickers();
    }
}
