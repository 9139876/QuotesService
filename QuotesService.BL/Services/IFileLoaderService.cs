using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.BL.Services
{
    public interface IFileLoaderService
    {
        Task<string> GetFileTextAsync(string fileName);
    }
}
