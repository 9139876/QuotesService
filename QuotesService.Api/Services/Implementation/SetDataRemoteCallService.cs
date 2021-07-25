﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.RemoteCall;
using CommonLibraries.RemoteCall.Services;
using Microsoft.Extensions.Configuration;
using QuotesService.Api.Models.RequestResponse;

namespace QuotesService.Api.Services.Implementation
{
    internal class SetDataRemoteCallService : BaseRemoteCallService, ISetDataRemoteCallService
    {
        public SetDataRemoteCallService(
            IConfiguration configuration,
            IRemoteCallHelperService remoteCallHelperService) : base(configuration, remoteCallHelperService) { }

        protected override string _apiSchemeAndHostConfigKey { get; set; } = "QuotesService.Api.SchemeAndHost";

        public async Task<AddMarketResponse> AddMarket(AddMarketRequest request)
            => await ExecutePostAsync<AddMarketResponse, AddMarketRequest>("api/set/add-market", request);
    }
}