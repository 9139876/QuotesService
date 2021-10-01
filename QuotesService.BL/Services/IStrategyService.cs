using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotesService.Api.Enum;

namespace QuotesService.BL.Services
{
    public interface IStrategyService
    {
        IQuotesProvider GetInstance(QuotesProviderTypeEnum type);
    }
}
