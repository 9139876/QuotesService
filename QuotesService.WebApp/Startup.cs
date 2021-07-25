using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Web;

namespace QuotesService.WebApp
{
    public class Startup : CommonLibraryStartup
    {
        protected override void ConfigureApplication(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureWebApp(app, env);
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {

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
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
