using System;
using QuotesService.Api.Enum;

namespace QuotesService.BL.Services.Implementation
{
    internal class StrategyService : IStrategyService
    {
        private readonly IYahooFinanceService _yahooFinanceService;
        private readonly IStooqService _stooqService;

        public StrategyService(
            IYahooFinanceService yahooFinanceService,
            IStooqService stooqService)
        {
            _yahooFinanceService = yahooFinanceService;
            _stooqService = stooqService;
        }

        public IQuotesProvider GetInstance(QuotesProviderEnum type)
        {
            return type switch
            {
                QuotesProviderEnum.YahooFinance => _yahooFinanceService,
                QuotesProviderEnum.AlphaVantage => throw new NotImplementedException(),
                QuotesProviderEnum.Finam => throw new NotImplementedException(),
                QuotesProviderEnum.Finnhub => throw new NotImplementedException(),
                QuotesProviderEnum.Stooq => _stooqService,

                _ => throw new NotSupportedException($"Неизвестный тип поставщика котировок - {type.ToString()}"),
            };
        }
    }
}
