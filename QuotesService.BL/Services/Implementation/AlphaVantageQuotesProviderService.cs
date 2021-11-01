using CommonLibraries.Graal.Enums;
using CommonLibraries.Graal.Models;
using QuotesService.Api.Enum;
using QuotesService.Api.Models;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.BL.Enums;
using QuotesService.BL.Models;
using QuotesService.BL.Static;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.BL.Services.Implementation
{
    internal class AlphaVantageQuotesProviderService : AbstractQuotesProvider<AlphaVantageGetDataInfoModel>, IAlphaVantageQuotesProviderService
    {
        public AlphaVantageQuotesProviderService(
            ITickersRepository tickersRepository,
            ITickerTFsRepository tickerTFsRepository,
            IQuotesProvidersTasksRepository quotesProvidersTasksRepository,
            IQuotesDbContext quotesDbContext,
            IQuotesRepository quotesRepository) : base(
                tickersRepository,
                tickerTFsRepository,
                quotesProvidersTasksRepository,
                quotesDbContext)
        {
            _quotesRepository = quotesRepository;
        }

        private const int _compactSizeQuotesCount = 100;

        private readonly string _apiKey = "TDE3V7KNU8JUG893";
        private readonly IQuotesRepository _quotesRepository;

        protected override QuotesProviderTypeEnum QuotesProviderType => QuotesProviderTypeEnum.AlphaVantage;

        public override List<TimeFrameEnum> GetAvailableTimeFrames()
        {
            return new List<TimeFrameEnum>()
            {
                TimeFrameEnum.min1,
                TimeFrameEnum.H1,
                TimeFrameEnum.D1,
                TimeFrameEnum.W1,
                TimeFrameEnum.M1
            };
        }

        public override async Task<GetQuotesResponse> GetLastBatchQuotes(TickerMarketTimeFrame request)
        {
            var lastQuote = await _quotesRepository.GetLastQuote(request);

            var getQuotesRequest = new GetQuotesWithQPRequest()
            {
                QuotesProvider = QuotesProviderType,
                TickerMarketTimeFrame = request
            };

            if (lastQuote != null)
            {
                getQuotesRequest.StartDate = lastQuote.Date;
            }

            var getQuotesResponse = await GetQuotes(getQuotesRequest);

            var result = new GetQuotesResponse() { Quotes = AuxiliaryBL.CorrectQuotes(new QuotesCorrectRequest() { TimeFrame = request.TimeFrame, Quotes = getQuotesResponse }) };

            return result;
        }

        protected override AlphaVantageGetDataInfoModel CreateGetDataInfoModel(List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Where(x => x.Key == nameof(AlphaVantageGetDataInfoModel.FunctionPrefix)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(AlphaVantageGetDataInfoModel.FunctionPrefix)}");
            }

            var prefix = GetFunctionPrefix(parameters.Single(x => x.Key == nameof(AlphaVantageGetDataInfoModel.FunctionPrefix)).Value);

            var result = new AlphaVantageGetDataInfoModel()
            {
                FunctionPrefix = prefix.ToString()
            };

            switch (prefix)
            {
                case AlphaVantageFunctionsPrefixesEnum.TIME_SERIES:
                    {
                        FillStockDataInfoModel(result, parameters);
                        break;
                    }
                case AlphaVantageFunctionsPrefixesEnum.FX:
                    {
                        FillForexDataInfoModel(result, parameters);
                        break;
                    }
                case AlphaVantageFunctionsPrefixesEnum.DIGITAL_CURRENCY:
                    {
                        FillCryptoDataInfoModel(result, parameters);
                        break;
                    }

                default: throw new NotSupportedException($"Неизвестный префикс функции '{prefix}'");
            }

            return result;
        }

        protected override string GetQuotesURL(AlphaVantageGetDataInfoModel getDataInfo, DateTime? start, DateTime? end, TimeFrameEnum timeFrame)
        {
            var prefix = GetFunctionPrefix(getDataInfo.FunctionPrefix);

            var elements = new List<string>() { $"https://www.alphavantage.co/query?apikey={_apiKey}&datatype=csv" };

            elements.Add($"function={GetFunction(prefix.ToString(), timeFrame)}");
            elements.Add($"outputsize={GetOutputSize(start, timeFrame)}");

            switch (prefix)
            {
                case AlphaVantageFunctionsPrefixesEnum.TIME_SERIES:
                    {
                        elements.Add($"symbol={getDataInfo.Symbol}");
                        break;
                    }
                case AlphaVantageFunctionsPrefixesEnum.FX:
                    {
                        elements.Add($"from_symbol={getDataInfo.FromSymbol}");
                        elements.Add($"to_symbol={getDataInfo.ToSymbol}");
                        break;
                    }
                case AlphaVantageFunctionsPrefixesEnum.DIGITAL_CURRENCY:
                    {
                        elements.Add($"market={getDataInfo.Market}");
                        break;
                    }

                default: throw new NotSupportedException($"Неизвестный префикс функции '{prefix}'");
            }

            return string.Join('&', elements);
        }

        protected override List<QuoteModel> ParseQuotes(string data)
        {
            var result = new List<QuoteModel>();

            foreach (var row in data.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                var values = row.Split(',');

                result.Add(new QuoteModel()
                {
                    Date = DateTime.Parse(values[0]),
                    Open = decimal.Parse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Hi = decimal.Parse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Low = decimal.Parse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Close = decimal.Parse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Volume = decimal.Parse(values[5], NumberStyles.Any, CultureInfo.InvariantCulture),
                });
            }

            return result;
        }

        private static string GetFunction(string functionPrefix, TimeFrameEnum timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrameEnum.min1:
                case TimeFrameEnum.H1:
                    {
                        return functionPrefix + "_INTRADAY";
                    }
                case TimeFrameEnum.D1:
                    {
                        return functionPrefix + "_DAILY";

                    }
                case TimeFrameEnum.W1:
                    {
                        return functionPrefix + "_WEEKLY";
                    }
                case TimeFrameEnum.M1:
                    {
                        return functionPrefix + "_MONTHLY";
                    }

                default: throw new NotSupportedException($"Таймфрейм {timeFrame} не поддерживается поставщиком котировок.");
            }
        }

        private static AlphaVantageFunctionsPrefixesEnum GetFunctionPrefix(string str)
        {
            var res = Enum.GetValues<AlphaVantageFunctionsPrefixesEnum>().Where(x => str.StartsWith(x.ToString()));

            if (res.Count() != 1)
            {
                throw new InvalidOperationException($"Не удалось извлечь префикс функции из '{str}'");
            }

            return res.Single();
        }

        private static void FillStockDataInfoModel(AlphaVantageGetDataInfoModel gdim, List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Where(x => x.Key == nameof(AlphaVantageGetDataInfoModel.Symbol)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(AlphaVantageGetDataInfoModel.Symbol)}");
            }

            gdim.Symbol = parameters.Single(x => x.Key == nameof(AlphaVantageGetDataInfoModel.Symbol)).Value;
        }

        private static void FillForexDataInfoModel(AlphaVantageGetDataInfoModel gdim, List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Where(x => x.Key == nameof(AlphaVantageGetDataInfoModel.FromSymbol)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(AlphaVantageGetDataInfoModel.FromSymbol)}");
            }

            if (parameters.Where(x => x.Key == nameof(AlphaVantageGetDataInfoModel.ToSymbol)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(AlphaVantageGetDataInfoModel.ToSymbol)}");
            }

            gdim.FromSymbol = parameters.Single(x => x.Key == nameof(AlphaVantageGetDataInfoModel.FromSymbol)).Value;
            gdim.ToSymbol = parameters.Single(x => x.Key == nameof(AlphaVantageGetDataInfoModel.ToSymbol)).Value;
        }

        private static void FillCryptoDataInfoModel(AlphaVantageGetDataInfoModel gdim, List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Where(x => x.Key == nameof(AlphaVantageGetDataInfoModel.Market)).Any() == false)
            {
                throw new InvalidOperationException($"Пропущен обязательный параметр - {nameof(AlphaVantageGetDataInfoModel.Market)}");
            }

            gdim.Market = parameters.Single(x => x.Key == nameof(AlphaVantageGetDataInfoModel.Market)).Value;
        }

        private static string GetOutputSize(DateTime? dateFrom, TimeFrameEnum timeFrame)
        {
            if (AuxiliaryBL.GetDifferenceBetweenDates(dateFrom ?? DateTime.MinValue, DateTime.Now, timeFrame) < _compactSizeQuotesCount)
            {
                return "compact";
            }
            else
            {
                return "full";
            }
        }
    }
}
