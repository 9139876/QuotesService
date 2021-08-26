using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Repositories;
using QuotesService.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Controllers.Api
{
    [Route("quotes-provider-tasks")]
    public class QuotesProviderTasksController : Controller
    {
        private readonly ITickersRepository _tickersRepository;
        private readonly IQuotesProvidersTasksRepository _quotesProvidersTasksRepository;
        private readonly IQuotesDbContext _quotesDbContext;

        public QuotesProviderTasksController(
            ITickersRepository tickersRepository,
            IQuotesProvidersTasksRepository quotesProvidersTasksRepository,
            IQuotesDbContext quotesDbContext)
        {
            _tickersRepository = tickersRepository;
            _quotesProvidersTasksRepository = quotesProvidersTasksRepository;
            _quotesDbContext = quotesDbContext;
        }

        [HttpPost("get-quotes-provider-tasks")]
        public async Task<QuotesProviderTasksModel> GetQuotesProviderTasks([FromBody] TickerAndMarketRequest request)
        {
            request.RequiredNotNull(nameof(request));

            if (string.IsNullOrEmpty(request.MarketName) || string.IsNullOrEmpty(request.TickerName))
            {
                return new QuotesProviderTasksModel()
                {
                    Status = new StandartResponse()
                    {
                        IsSuccess = false,
                        Message = $"Имя {(string.IsNullOrEmpty(request.MarketName) ? "рынка" : "инструмента") } не может быть пустым"
                    }
                };
            }

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(request);

            if (existingTicker == null)
            {
                return new QuotesProviderTasksModel()
                {
                    Status = new StandartResponse()
                    {
                        IsSuccess = false,
                        Message = $"Инструмент {request.TickerName} для рынка {request.MarketName} не существует"
                    }
                };
            }

            var tasks = await _quotesProvidersTasksRepository.GetTasksByTickerId(existingTicker.Id);

            var result = new QuotesProviderTasksModel()
            {
                Status = new StandartResponse() { IsSuccess = true },
                MarketName = request.MarketName,
                TickerName = request.TickerName,
                QuotesProviderTasks = tasks
                    .Select(x => new QuotesProviderTask()
                    {
                        IsActive = x.Value.IsActive,
                        UpdatePeriodInSecond = x.Value.UpdatePeriodInSecond,
                        LastUpdateDate = x.Value.LastUpdateDate?.ToString() ?? string.Empty,
                        TimeFrameName = Description.GetDescription(x.Key)
                    })
                    .ToList()
            };

            return result;
        }

        [HttpPost("set-quotes-provider-tasks")]
        public async Task<StandartResponse> SetQuotesProviderTasks([FromBody] QuotesProviderTasksModel request)
        {
            request.RequiredNotNull(nameof(request));

            if (string.IsNullOrEmpty(request.MarketName) || string.IsNullOrEmpty(request.TickerName))
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Имя {(string.IsNullOrEmpty(request.MarketName) ? "рынка" : "инструмента") } не может быть пустым"
                };
            }

            var existingTicker = await _tickersRepository.GetByTickerAndMarket(new TickerAndMarketRequest()
            {
                MarketName = request.MarketName,
                TickerName = request.TickerName
            });

            if (existingTicker == null)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = $"Инструмент {request.TickerName} для рынка {request.MarketName} не существует"
                };
            }

            var tasks = await _quotesProvidersTasksRepository.GetTasksByTickerId(existingTicker.Id);

            var changedTasks = new List<QuotesProviderTaskEntity>();

            foreach(var task in tasks)
            {
                var serialize = task.Value.Serialize();

                var newValue = request.QuotesProviderTasks.SingleOrDefault(x => x.TimeFrameName == Description.GetDescription(task.Key));

                if (newValue != null)
                {
                    task.Value.IsActive = newValue.IsActive;
                    task.Value.UpdatePeriodInSecond = newValue.UpdatePeriodInSecond;

                    if(task.Value.Serialize() != serialize)
                    {
                        changedTasks.Add(task.Value);
                    }
                }
            }

            if (changedTasks.Any())
            {
                using (var transaction = _quotesDbContext.BeginTransaction())
                {
                    foreach(var task in changedTasks)
                    {
                        await _quotesProvidersTasksRepository.UpdateAsync(task);
                    }

                    transaction.Commit();
                }
            }

            return new StandartResponse() { IsSuccess = true };
        }
    }
}
