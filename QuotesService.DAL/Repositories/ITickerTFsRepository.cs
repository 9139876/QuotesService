using CommonLibraries.EF;
using CommonLibraries.Graal.Enums;
using QuotesService.DAL.Entities;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface ITickerTFsRepository : IBaseRepository<TickerTFEntity>
    {
        Task<TickerTFEntity> GetByTickerIdAndTF(int tickerId, TimeFrameEnum tf);
    }
}
