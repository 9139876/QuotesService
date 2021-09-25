using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Internal.Implementation;

namespace QuotesService.DAL
{
    public static class RegisterDbContext
    {
        public static void RegisterDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterPostgreSQLDbContext<IQuotesDbContext, QuotesDbContext>(() => configuration.GetConnectionString("QuotesDb"));
        }
    }
}
