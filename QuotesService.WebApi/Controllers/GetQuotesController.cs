using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Repositories;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using QuotesService.Api.Models;

namespace QuotesService.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class GetQuotesController
    {
        private readonly IQuotesRepository _quotesRepository;

        public GetQuotesController(
            IQuotesRepository quotesRepository)
        {
            _quotesRepository = quotesRepository;
        }

        [HttpPost]
        [Route("get-quotes-info")]
        public async Task<QuotesInfoResponse> GetQuotesInfo([FromBody] TickerMarketTimeFrame request)
        {
            request.RequiredNotNull(nameof(request));

            var quotes = await _quotesRepository.GetAllQuotes(request);
            quotes.RequiredNotNull(nameof(quotes), request);

            var count = quotes.Count;

            if (count == 0)
            {
                return new QuotesInfoResponse() { QuotesCount = 0 };
            }

            var firstDate = quotes.Min(x => x.Date);
            var lastDate = quotes.Max(x => x.Date);

            var minPriceQuote = quotes.OrderBy(x => x.Low).First();
            var maxPriceQuote = quotes.OrderByDescending(x => x.Hi).First();

            var result = new QuotesInfoResponse()
            {
                QuotesCount = count,
                FirstDate = firstDate,
                LastDate = lastDate,
                MinPriceDate = minPriceQuote.Date,
                MinPrice = minPriceQuote.Low,
                MaxPriceDate = maxPriceQuote.Date,
                MaxPrice = maxPriceQuote.Hi
            };

            return result;
        }

        [HttpPost]
        [Route("get-quotes")]
        public async Task<List<QuoteModel>> GetQuotes([FromBody]GetQuotesRequest request)
        {
            request.RequiredNotNull(nameof(request));

            var quotes = await _quotesRepository.GetQuotes(request);
            quotes.RequiredNotNull(nameof(quotes), request);

            return quotes
                .Select(x => new QuoteModel()
                {
                    Date = x.Date,
                    Open = x.Open ?? (x.Hi + x.Low) / 2,
                    Hi = x.Hi,
                    Low = x.Low,
                    Close = x.Close ?? (x.Hi + x.Low) / 2,
                    Volume = x.Volume ?? -1
                })
                .ToList();
        }
    }
}
