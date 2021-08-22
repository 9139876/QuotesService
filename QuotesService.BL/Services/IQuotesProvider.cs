﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.ApiPrivate.Models.RequestResponse;

namespace QuotesService.BL.Services
{
    public interface IQuotesProvider
    {
        Task<GetQuotesResponse> GetQuotes(GetQuotesRequest request);

        Task<CheckGetQuotesResponse> CheckGetQuotes(CheckGetQuotesRequest request);

        Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request);

        Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request);
    }
}
