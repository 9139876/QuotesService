using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class TickersInfoesNamesRepository : BaseRepository<TickerInfoNameEntity>, ITickersInfoesNamesRepository
    {
        private readonly IQuotesDbContext _quotesDbContext;

        public TickersInfoesNamesRepository(IQuotesDbContext quotesDbContext) : base(quotesDbContext.TickersInfoesNames)
        {
            _quotesDbContext = quotesDbContext;
        }

        public async Task<List<string>> GetAllNames()
        {
            return await DbSet.Select(x => x.Name).ToListAsync();
        }
    }
}
