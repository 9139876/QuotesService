using CommonLibraries.EF;
using CommonLibraries.Graal.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface ITickersRepository : IBaseRepository<TickerEntity>
    {
        Task<TickerEntity> GetByTickerAndMarket(TickerAndMarket request);

        Task<List<TickerEntity>> GetAllTickers();

        Task<List<TickerAndMarket>> GetAllTickersAndMarkets(GetAllTickersAndMarketsRequest request);
    }
}
