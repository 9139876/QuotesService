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
        private readonly IMarketsRepository _marketEntityRepository;

        public SetDataController(
            IMarketsRepository marketEntityRepository)
        {
            _marketEntityRepository = marketEntityRepository;
        }

        [HttpPost]
        [Route("add-market")]
        public async Task<AddMarketResponse> AddMarket([FromBody] AddMarketRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var existingMarket = (await _marketEntityRepository.GetAllMarketsNames()).FirstOrDefault(x => x == request.MarketName);

            if (existingMarket != null)
            {
                return new AddMarketResponse()
                {
                    IsSuccess = false,
                    Message = $"Рынок с именем {request.MarketName} уже существует"
                };
            }

            var marketEntity = new MarketEntity()
            {
                Name = request.MarketName
            };

            await _marketEntityRepository.InsertAsync(marketEntity);

            return new AddMarketResponse() { IsSuccess = true };
        }
    }
}
