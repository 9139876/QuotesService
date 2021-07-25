using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.ClientApplication;
using Microsoft.Extensions.Configuration;

namespace QuotesService.GUI
{
    internal class Startup : CommonLibraries.ClientApplication.ClientApplicationStartup
    {
        protected override void ConfigureServices()
        {
            //Program.Configuration = ServicesFactory.GetInstance<IConfiguration>();

            CommonLibraries.ClientApplication.ServicesFactory.RegisterAssemblyServiceAndRepositoryByMember<QuotesService.Api.PlaceboRegistration>();
        }
    }
}
