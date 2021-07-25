using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Repositories;

namespace QuotesService.WebApi.Controllers
{
    [Route("api/set")]
    [ApiController]
    public class SetDataController
    {
        private readonly IMarketEntityRepository _marketEntityRepository;

        public SetDataController(
            IMarketEntityRepository marketEntityRepository)
        {
            _marketEntityRepository = marketEntityRepository;
        }

        [HttpPost]
        [Route("add-market")]
        public async Task<AddMarketResponse> AddMarket([FromBody] AddMarketRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var existingMarket = await _marketEntityRepository.GetByQuotesProviderAndName(request.QuotesProvider, request.MarketName);

            if (existingMarket != null)
            {
                return new AddMarketResponse()
                {
                    IsSuccess = false,
                    Message = $"Рынок {request.MarketName} уже существует для {request.QuotesProvider.ToString()}"
                };
            }

            var marketEntity = new MarketEntity()
            {
                Name = request.MarketName,
                QuotesProvider = request.QuotesProvider
            };

            await _marketEntityRepository.InsertAsync(marketEntity);

            return new AddMarketResponse() { IsSuccess = true };
        }
    }
}
