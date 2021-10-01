using System;
using QuotesService.Api.Enum;

namespace QuotesService.BL.Services.Implementation
{
    internal class StrategyService : IStrategyService
    {
        private readonly IYahooFinanceQuotesProviderService _yahooFinanceService;
        private readonly IStooqQuotesProviderService _stooqService;
        private readonly INasdaqQuotesProviderService _nasdaqQuotesProviderService;
        private readonly IAlphaVantageQuotesProviderService _alphaVantageQuotesProviderService;

        public StrategyService(
            IYahooFinanceQuotesProviderService yahooFinanceService,
            IStooqQuotesProviderService stooqService,
            INasdaqQuotesProviderService nasdaqQuotesProviderService,
            IAlphaVantageQuotesProviderService alphaVantageQuotesProviderService)
        {
            _yahooFinanceService = yahooFinanceService;
            _stooqService = stooqService;
            _nasdaqQuotesProviderService = nasdaqQuotesProviderService;
            _alphaVantageQuotesProviderService = alphaVantageQuotesProviderService;
        }

        public IQuotesProvider GetInstance(QuotesProviderTypeEnum type)
        {
            return type switch
            {
                QuotesProviderTypeEnum.YahooFinance => _yahooFinanceService,
                QuotesProviderTypeEnum.AlphaVantage => _alphaVantageQuotesProviderService,
                QuotesProviderTypeEnum.Finam => throw new NotImplementedException(),
                QuotesProviderTypeEnum.Nasdaq => _nasdaqQuotesProviderService,
                QuotesProviderTypeEnum.Stooq => _stooqService,

                _ => throw new NotSupportedException($"Неизвестный тип поставщика котировок - {type.ToString()}"),
            };
        }
    }
}
