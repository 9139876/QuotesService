using Microsoft.AspNetCore.Mvc;
using QuotesService.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Services
{
    public interface IQuotesFileLoaderService
    {
        Task<string> GetFileTextAsync(string fileName);

        TryParseDateResponse DateTimeTryParse([FromBody] TryParseDateRequest request);
    }
}
