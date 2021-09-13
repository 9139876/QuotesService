using CommonLibraries.Core.Extensions;
using Microsoft.Extensions.Configuration;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.BL.Services;
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
        private readonly IQuotesProvidersTasksRepository _quotesProvidersTasksRepository;
        private readonly IStrategyService _strategyService;
        private readonly IQuotesStorageService _quotesStorageService;
        private readonly bool _isUpdateDifferenceQuotes;

        public GetQuotesBackgroundService(
            IQuotesProvidersTasksRepository quotesProvidersTasksRepository,
            IStrategyService strategyService,
            IQuotesStorageService quotesStorageService,
            IConfiguration configuration
            )
        {
            _quotesProvidersTasksRepository = quotesProvidersTasksRepository;
            _strategyService = strategyService;
            _quotesStorageService = quotesStorageService;
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

            var quotesRequest = new TickerMarketTimeFrame()
            {
                TickerName = tickerAndMarket.TickerName,
                MarketName = tickerAndMarket.MarketName,
                TimeFrame = quotesProviderTimeFrame,
            };

            var quotes = (await service.GetLastBatchQuotes(quotesRequest))?.Quotes;
            quotes.RequiredNotNull(nameof(quotes), quotesRequest);

            var quotesToStorageRequest = new QuotesToStorageRequest()
            {
                Quotes = quotes,
                TickerMarketTimeFrame = new TickerMarketTimeFrame()
                {
                    TickerName = tickerAndMarket.TickerName,
                    MarketName = tickerAndMarket.MarketName,
                    TimeFrame = quotesProviderTimeFrame
                },
                TickerTfId = task.TickerTFId,
                IsUpdateDifferenceQuotes = _isUpdateDifferenceQuotes
            };

            var lastQuoteIsNow = await _quotesStorageService.QuotesToStorage(quotesToStorageRequest);

            //Если последняя полученная котировка далека от сегодняшней даты - не ставим LastUpdateDate,
            //чтобы шедулер не ждал UpdatePeriodInSecond а быстрее собирал котировки
            if (task.LastUpdateDate != null || lastQuoteIsNow)
            {
                task.LastUpdateDate = DateTime.Now;
                await _quotesProvidersTasksRepository.UpdateAsync(task);
            }
        }
    }
}
