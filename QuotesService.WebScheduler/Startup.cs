using CommonLibraries.BackgroundWorker.Extensions;
using CommonLibraries.Core.Extensions;
using CommonLibraries.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuotesService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebScheduler
{
    public class Startup : CommonLibraryStartup
    {
        public Startup() : base()
        { }

        protected override void ConfigurePipelineAfterExceptionsHandling(IApplicationBuilder app)
        {
        }

        protected override void ConfigurePipelineAfterMvc(IApplicationBuilder app)
        {
        }

        protected override void ConfigureRoutes(IApplicationBuilder app)
        {

        }

        protected override void ConfigureApplication(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureWebApi(app, env);
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {
            services.RegisterDbContexts(Configuration);

            services.AddBackgroundWorkers<Startup>(Configuration);

            services.RegisterAssemblyServiceAndRepositoryByMember<BL.PlaceboRegistration>();
            services.RegisterAssemblyServiceAndRepositoryByMember<DAL.PlaceboRegistration>();
            services.RegisterAssemblyServiceAndRepositoryByMember<Startup>();
        }
    }
}
