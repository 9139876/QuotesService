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
            switch (type)
            {
                case QuotesProviderEnum.YahooFinance: return _yahooFinanceService;
                case QuotesProviderEnum.AlphaVantage: throw new NotImplementedException();
                case QuotesProviderEnum.Finam: throw new NotImplementedException();
                case QuotesProviderEnum.Finnhub: throw new NotImplementedException();
                case QuotesProviderEnum.Stooq: throw new NotImplementedException();

                default: throw new NotSupportedException($"Неизвестный тип поставщика котировок - {type.ToString()}");
            }
        }
    }
}
