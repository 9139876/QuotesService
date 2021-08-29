using CommonLibraries.Core.Extensions;
using Microsoft.Extensions.Configuration;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.BL.Services;
using QuotesService.BL.Static;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuotesService.WebScheduler.Services.Implementation
{
    internal class GetQuotesBackgroundService : IGetQuotesBackgroundService
    {
        private readonly IQuotesDbContext _quotesDbContext;
        private readonly IQuotesRepository _quotesRepository;
        private readonly IQuotesProvidersTasksRepository _quotesProvidersTasksRepository;
        private readonly IStrategyService _strategyService;

        private readonly bool _isUpdateDifferenceQuotes;

        public GetQuotesBackgroundService(
            IQuotesDbContext quotesDbContext,
            IQuotesRepository quotesRepository,
            IQuotesProvidersTasksRepository quotesProvidersTasksRepository,
            IStrategyService strategyService,
            IConfiguration configuration
            )
        {
            _quotesDbContext = quotesDbContext;
            _quotesRepository = quotesRepository;
            _quotesProvidersTasksRepository = quotesProvidersTasksRepository;
            _strategyService = strategyService;
            _isUpdateDifferenceQuotes = configuration.GetValue<bool>("IsUpdateDifferenceQuotes");
            _isUpdateDifferenceQuotes = true;
        }

        public async Task<List<int>> GetItemsAsync()
        {
            var tasks = await _quotesProvidersTasksRepository.GetAllActiveTasks();

            tasks.RequiredNotNull(nameof(tasks));

            var now = DateTime.Now;

            return tasks
                .Where(x => x.LastUpdateDate == null || (x.LastUpdateDate ?? DateTime.MaxValue).AddSeconds(x.UpdatePeriodInSecond) < now)
                .Select(x => x.Id)
                .ToList();
        }

        public async Task ProcessItemAsync(int item, CancellationToken stoppinngToken)
        {
            var task = await _quotesProvidersTasksRepository.GetTaskById(item);
            task.RequiredNotNull(nameof(task), item);

            var quotesProviderType = await _quotesProvidersTasksRepository.GetQuotesProviderType(item);

            var service = _strategyService.GetInstance(quotesProviderType);

            var quotesProviderTimeFrame = await _quotesProvidersTasksRepository.GetQuotesProviderTimeFrame(item);

            var tickerAndMarket = await _quotesProvidersTasksRepository.GetQuotesProviderTickerAndMarket(item);
            tickerAndMarket.RequiredNotNull(nameof(tickerAndMarket), item);

            var quotesRequest = new GetBatchQuotesRequest()
            {
                TimeFrame = quotesProviderTimeFrame,
                MarketName = tickerAndMarket.MarketName,
                TickerName = tickerAndMarket.TickerName
            };

            var quotes = (await service.GetLastBatchQuotes(quotesRequest))?.Quotes;
            quotes.RequiredNotNull(nameof(quotes), quotesRequest);

            quotes = QuotesAuxiliary.CorrectQuotes(new QuotesCorrectRequest() { TimeFrame = quotesProviderTimeFrame, Quotes = quotes });

            var getExistingQuotesRequest = new GetQuotesRequest()
            {
                TimeFrame = quotesProviderTimeFrame,
                MarketName = tickerAndMarket.MarketName,
                TickerName = tickerAndMarket.TickerName,
                StartDate = quotes.Select(x => x.Date).Min(),
                EndDate = quotes.Select(x => x.Date).Max()
            };

            var existingQuotes = await _quotesRepository.GetQuotes(getExistingQuotesRequest);

            //Пока никаких проверок, просто обновляем если надо!!!
            //Обновлять котировки, если новые пришли с изменениями

            using (var transaction = _quotesDbContext.BeginTransaction())
            {
                foreach (var quote in quotes)
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
                            ParentTickerTFId = task.TickerTFId
                        });
                    }
                    else
                    {
                        if (_isUpdateDifferenceQuotes)
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
                    }
                }

                //Если последняя полученная котировка далека от сегодняшней даты - не ставим LastUpdateDate,
                //чтобы шедулер не ждал UpdatePeriodInSecond а быстрее собирал котировки
                if (task.LastUpdateDate != null || QuotesAuxiliary.IsReallyLastQuote(quotes.Max(x => x.Date), quotesProviderTimeFrame))
                {
                    task.LastUpdateDate = DateTime.Now;
                    await _quotesProvidersTasksRepository.UpdateAsync(task);
                }

                transaction.Commit();
            }
        }
    }
}
