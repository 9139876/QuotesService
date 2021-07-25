using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class QuotesProviderEntityRepository : BaseRepository<QuotesProviderEntity>, IQuotesProviderEntityRepository
    {
        private readonly IMarketsDbContext _dbcontext;

        public QuotesProviderEntityRepository(IMarketsDbContext dbcontext) : base(dbcontext.QuotesProviderEntity)
        {
            _dbcontext = dbcontext;
        }

        public List<QuotesProviderEntity> GetAllQuotesProviders()
        {
            return DbSet.ToList();
        }

        public async Task<QuotesProviderEntity> GetQuotesProviderByName(string name)
        {
            return await DbSet.SingleOrDefaultAsync(x => x.Name == name);
        }
    }
}
