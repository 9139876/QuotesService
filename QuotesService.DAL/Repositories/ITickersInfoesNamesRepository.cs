using CommonLibraries.EF;
using QuotesService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories
{
    public interface ITickersInfoesNamesRepository : IBaseRepository<TickerInfoNameEntity>
    {
        Task<List<string>> GetAllNames();
    }
}
