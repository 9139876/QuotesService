using CommonLibraries.Core.Extensions;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Models;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuotesService.BL.Services.Implementation
{
    internal abstract class AbstractQuotesProvider<TGetDataInfoModel> : IQuotesProvider where TGetDataInfoModel : class, new()
    {
        protected readonly ITickersRepository _tickersRepository;
        protected readonly ITickerTFsRepository _tickerTFsRepository;
        protected readonly IQuotesProvidersTasksRepository _quotesProvidersTasksRepository;
        protected readonly IQuotesDbContext _quotesDbContext;

        protected AbstractQuotesProvider(
            ITickersRepository tickersRepository,
            ITickerTFsRepository tickerTFsRepository,
            IQuotesProvidersTasksRepository quotesProvidersTasksRepository,
            IQuotesDbContext quotesDbContext)
        {
            _tickersRepository = tickersRepository;
            _tickerTFsRepository = tickerTFsRepository;
            _quotesProvidersTasksRepository = quotesProvidersTasksRepository;
            _quotesDbContext = quotesDbContext;
        }

        protected abstract QuotesProviderTypeEnum QuotesProviderType { get; }

        public abstract Task<GetQuotesResponse> GetLastBatchQuotes(TickerMarketTimeFrame request);

        public abstract List<TimeFrameEnum> GetAvailableTimeFrames();

        public async Task<StandartResponse> CheckGetQuotes(CheckGetQuotesRequest request)
        {
            try
            {
                var getDataInfoModel = CreateGetDataInfoModel(request.Parameters);

                var url = GetQuotesURL(getDataInfoModel, DateTime.Now.AddMonths(-1), DateTime.Now, TimeFrameEnum.D1);

                var quotes = await GetQuotes(url);

                if (quotes.Any())
                {
                    return new StandartResponse()
                    {
                        IsSuccess = true
                    };
                }
                else
                {
                    return new StandartResponse()
                    {
                        IsSuccess = false,
                        Message = $"В течение последнего месяца не было ни одной котировки"
                    };
                }
            }
            catch (Exception e)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<List<KeyValuePair<string, string>>> GetQuotesProviderParameters(GetQuotesProviderParametersRequest request)
        {
            var ticker = await GetTicker(request.TickerName, request.MarketName);

            var getDataInfo = ticker.ProviderGetDataInfo?.Deserialize<TGetDataInfoModel>() ?? new TGetDataInfoModel();

            return getDataInfo.ToPropertiesCollection();
        }

        public async Task<StandartResponse> SetQuotesProviderParameters(SetQuotesProviderParametersRequest request)
        {
            try
            {
                var ticker = await GetTicker(request.TickerName, request.MarketName);

                if (ticker.QuotesProviderType != null)
                {
                    throw new InvalidOperationException($"Для инструмента {request.TickerName} рынок {request.MarketName} уже установлены параметры поставщика котировок, изменять их нельзя, нужно создать новый инструмент");
                }

                ticker.QuotesProviderType = request.QuotesProviderType;
                ticker.ProviderGetDataInfo = CreateGetDataInfoModel(request.Parameters).Serialize();
                ticker.Name = $"{ticker.Name} ({QuotesProviderType.ToString()})";

                var tasks = new List<QuotesProviderTaskEntity>();

                foreach (var tf in GetAvailableTimeFrames())
                {
                    var ttf = await _tickerTFsRepository.GetByTickerIdAndTF(ticker.Id, tf);
                    ttf.RequiredNotNull(nameof(ttf), new { ticker.Id, tf });

                    tasks.Add(new QuotesProviderTaskEntity()
                    {
                        TickerTFId = ttf.Id,
                        UpdatePeriodInSecond = 3600,
                        IsActive = false
                    });
                }

                using (var transaction = _quotesDbContext.BeginTransaction())
                {
                    await _tickersRepository.UpdateAsync(ticker);

                    foreach (var task in tasks)
                    {
                        await _quotesProvidersTasksRepository.InsertAsync(task);
                    }

                    transaction.Commit();
                }

                return new StandartResponse() { IsSuccess = true, Message = ticker.Name };
            }
            catch (Exception ex)
            {
                return new StandartResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        #region protected

        protected abstract TGetDataInfoModel CreateGetDataInfoModel(List<KeyValuePair<string, string>> parameters);

        protected abstract string GetQuotesURL(TGetDataInfoModel getDataInfo, DateTime? start, DateTime? end, TimeFrameEnum timeFrame);

        protected abstract List<QuoteModel> ParseQuotes(string data);

        protected async Task<List<QuoteModel>> GetQuotes(GetQuotesRequest request)
        {
            var ticker = await GetTicker(request.TickerMarketTimeFrame.TickerName, request.TickerMarketTimeFrame.MarketName);

            var getDataInfo = ticker.ProviderGetDataInfo?.Deserialize<TGetDataInfoModel>();

            if (getDataInfo == null)
            {
                throw new InvalidOperationException($"Ticker not have {nameof(getDataInfo)} - {ticker.Serialize()}");
            }

            var url = GetQuotesURL(getDataInfo, request.StartDate, request.EndDate, request.TickerMarketTimeFrame.TimeFrame);

            return await GetQuotes(url);
        }

        protected virtual async Task<List<QuoteModel>> GetQuotes(string url)
        {
            var data = string.Empty;

            using (var reader = new System.IO.StreamReader(new WebClient().OpenRead(url) ?? throw new InvalidOperationException($"Не удалось получить данные по адресу '{url}'")))
            {
                data = await reader.ReadToEndAsync();
            }
            var response = ParseQuotes(data);
            return response;
        }

        protected async Task<TickerEntity> GetTicker(string tickerName, string marketName)
        {
            var tickerAndMarketRequest = new TickerAndMarket()
            {
                MarketName = marketName,
                TickerName = tickerName
            };

            var ticker = await _tickersRepository.GetByTickerAndMarket(tickerAndMarketRequest);

            if (ticker == null)
            {
                throw new InvalidOperationException($"Ticker not found for {tickerAndMarketRequest.Serialize()}");
            }

            return ticker;
        }

        #endregion
    }
}
