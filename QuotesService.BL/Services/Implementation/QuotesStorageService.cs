using CommonLibraries.Core.Extensions;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.BL.Static;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.BL.Services.Implementation
{
    internal class QuotesStorageService : IQuotesStorageService
    {
        private readonly IQuotesDbContext _quotesDbContext;
        private readonly IQuotesRepository _quotesRepository;

        public QuotesStorageService(
            IQuotesDbContext quotesDbContext,
            IQuotesRepository quotesRepository)
        {
            _quotesDbContext = quotesDbContext;
            _quotesRepository = quotesRepository;
        }

        public async Task<bool> QuotesToStorage(QuotesToStorageRequest request)
        {
            var getExistingQuotesRequest = new GetQuotesRequest()
            {
                TickerMarketTimeFrame = request.TickerMarketTimeFrame,
                StartDate = request.Quotes.Select(x => x.Date).Min(),
                EndDate = request.Quotes.Select(x => x.Date).Max()
            };

            var existingQuotes = await _quotesRepository.GetQuotes(getExistingQuotesRequest);

            //Пока никаких проверок, просто обновляем если надо!!!
            //Обновлять котировки, если новые пришли с изменениями

            using (var transaction = _quotesDbContext.BeginTransaction())
            {
                foreach (var quote in request.Quotes)
                {
                    var existingQuote = existingQuotes.SingleOrDefault(x => x.Date == quote.Date);

                    if (existingQuote == null)
                    {
                        await _quotesRepository.InsertAsync(new QuoteEntity()
                        {
                            Date = quote.Date,
                            Open = quote.Open,
                            Hi = quote.Hi,
                            Low = quote.Low,
                            Close = quote.Close,
                            Volume = quote.Volume,
                            ParentTickerTFId = request.TickerTfId
                        });
                    }
                    else
                    {
                        if (request.IsUpdateDifferenceQuotes)
                        {
                            var serialize = existingQuote.Serialize();

                            existingQuote.Date = quote.Date;
                            existingQuote.Open = quote.Open;
                            existingQuote.Hi = quote.Hi;
                            existingQuote.Low = quote.Low;
                            existingQuote.Close = quote.Close;
                            existingQuote.Volume = quote.Volume;

                            if (existingQuote.Serialize() != serialize)
                            {
                                await _quotesRepository.UpdateAsync(existingQuote);
                            }
                        }

                        //Запись в какой-нибудь лог!!!
                    }
                }

                transaction.Commit();
            }

            return Auxiliary.IsReallyLastQuote(request.Quotes.Max(x => x.Date), request.TickerMarketTimeFrame.TimeFrame);
        }
    }
}
