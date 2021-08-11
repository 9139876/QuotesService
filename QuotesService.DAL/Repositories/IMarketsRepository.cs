using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries.EF;
using QuotesService.Api.Enum;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Models;

namespace QuotesService.DAL.Repositories
{
    public interface IMarketsRepository : IBaseRepository<MarketEntity>
    {
        Task<List<MarketEntity>> GetAllMarkets();

        Task<MarketEntity> GetMarketByName(string name);
    }
}
