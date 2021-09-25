using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Core.Extensions;
using CommonLibraries.Web;
using QuotesService.DAL;

namespace QuotesService.WebApi
{
    public class Startup : CommonLibraryStartup
    {
        protected override bool _loadFromConfigService => true;

        protected override bool _reloadAppSettingsOnChange => true;

        protected override bool _requiredConfigService => true;

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

            services.RegisterAssemblyServiceAndRepositoryByMember<BL.PlaceboRegistration>();
            services.RegisterAssemblyServiceAndRepositoryByMember<DAL.PlaceboRegistration>();
            services.RegisterAssemblyServiceAndRepositoryByMember<Startup>();
        }
    }
}
