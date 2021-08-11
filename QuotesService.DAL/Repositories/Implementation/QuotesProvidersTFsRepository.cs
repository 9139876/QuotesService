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
    internal class QuotesProvidersTFsRepository : BaseRepository<QuotesProviderTFEntity>, IQuotesProvidersTFsRepository
    {
        private readonly IQuotesDbContext _dbcontext;

        public QuotesProvidersTFsRepository(IQuotesDbContext dbcontext) : base(dbcontext.QuotesProviderTFs)
        {
            _dbcontext = dbcontext;
        }
    }
}
