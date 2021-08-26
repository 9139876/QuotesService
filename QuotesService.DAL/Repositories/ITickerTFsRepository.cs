using CommonLibraries.EF;
using QuotesService.Api.Enum;
using QuotesService.DAL.Entities;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface ITickerTFsRepository : IBaseRepository<TickerTFEntity>
    {
        Task<TickerTFEntity> GetByTickerIdAndTF(int tickerId, TimeFrameEnum tf);
    }
}
