using QuotesService.Api.Models;
using QuotesService.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.BL.Services
{
    public interface IQuotesStorageService
    {
        Task<bool> QuotesToStorage(QuotesToStorageRequest request);
    }
}
