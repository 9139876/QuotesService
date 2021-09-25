using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CommonLibraries.Web;
using CommonLibraries.Core.Extensions;
using QuotesService.DAL;

namespace QuotesService.WebApp
{
    public class Startup : CommonLibraryStartup
    {
        protected override bool _loadFromConfigService => true;

        protected override bool _reloadAppSettingsOnChange => true;

        protected override bool _requiredConfigService => true;

        protected override void ConfigureApplication(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureWebAppStaticFiles(app, env);
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {
            services.RegisterDbContexts(Configuration);

            services.RegisterAssemblyServiceAndRepositoryByMember<BL.PlaceboRegistration>();
            services.RegisterAssemblyServiceAndRepositoryByMember<DAL.PlaceboRegistration>();
            services.RegisterAssemblyServiceAndRepositoryByMember<Startup>();
        }

        protected override void ConfigurePipelineAfterExceptionsHandling(IApplicationBuilder app)
        {

        }

        protected override void ConfigurePipelineAfterMvc(IApplicationBuilder app)
        {

        }

        protected override void ConfigureRoutes(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    //endpoints.MapControllerRoute(
                    //    name: "default",
                    ////pattern: "{controller=Home}/{action=Index}/{id?}");
                    //pattern: "{controller=Main}/{action=Index}/{id?}");
                    ////pattern: "{controller=QuotesGetter}/{action=Main}/{id?}");
                });
        }
    }
}
