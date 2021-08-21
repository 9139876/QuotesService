using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;

namespace QuotesService.Api.Services
{
    public interface IGetDataRemoteCallService
    {
        Task<CheckGetQuotesResponse> CheckGetQuotes(CheckGetQuotesRequest request);

        Task<GetQuotesResponse> GetQuotes(GetQuotesRequest request);

        Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request);

        Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request);
    }
}
