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
    internal class QuotesProvidersTasksRepository : BaseRepository<QuotesProviderTaskEntity>, IQuotesProvidersTasksRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public QuotesProvidersTasksRepository(IQuotesDbContext dbcontext) : base(dbcontext.QuotesProvidersTasks)
        {
            _dbcontext = dbcontext;
        }
    }
}
