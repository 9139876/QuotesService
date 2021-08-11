using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;

namespace QuotesService.DAL.Repositories.Implementation
{
    internal class QuotesProvidersRepository : BaseRepository<QuotesProviderEntity>, IQuotesProvidersRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public QuotesProvidersRepository(IQuotesDbContext dbcontext) : base(dbcontext.QuotesProviders)
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
