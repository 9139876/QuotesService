using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotesService.Api.Enum;

namespace QuotesService.BL.Services.Implementation
{
    internal class StrategyService : IStrategyService
    {
        private readonly IYahooFinanceService _yahooFinanceService;

        public StrategyService(
            IYahooFinanceService yahooFinanceService)
        {
            _yahooFinanceService = yahooFinanceService;
        }

        public IQuotesProvider GetInstance(QuotesProviderEnum type)
        {
            return type switch
            {
                QuotesProviderEnum.YahooFinance => _yahooFinanceService,
                QuotesProviderEnum.AlphaVantage => throw new NotImplementedException(),
                QuotesProviderEnum.Finam => throw new NotImplementedException(),
                QuotesProviderEnum.Finnhub => throw new NotImplementedException(),
                QuotesProviderEnum.Stooq => throw new NotImplementedException(),
                
                _ => throw new NotSupportedException($"Неизвестный тип поставщика котировок - {type.ToString()}"),
            };
        }
    }
}
