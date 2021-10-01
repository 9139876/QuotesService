using CommonLibraries.Core.Extensions;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.Api.Static;
using QuotesService.BL.Models;
using QuotesService.DAL.Entities;
using QuotesService.DAL.Internal;
using QuotesService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<CompareQuotesResponse> CompareQuotes(CompareQuotesRequest request)
        {
            var firstTickerQuotesRequest = new TickerMarketTimeFrame()
            {
                MarketName = request.TickerAndMarketFirst.MarketName,
                TickerName = request.TickerAndMarketFirst.TickerName,
                TimeFrame = request.TimeFrame
            };

            var firstTickerQuotes = await _quotesRepository.GetAllQuotes(firstTickerQuotesRequest);
            firstTickerQuotes.RequiredNotNull(nameof(firstTickerQuotes), firstTickerQuotesRequest);

            if (firstTickerQuotes.Any() == false)
            {
                return new CompareQuotesResponse()
                {
                    IsSuccess = false,
                    Message = "Не найдено котировок для первого инструмента"
                };
            }

            var secondTickerQuotesRequest = new TickerMarketTimeFrame()
            {
                MarketName = request.TickerAndMarketSecond.MarketName,
                TickerName = request.TickerAndMarketSecond.TickerName,
                TimeFrame = request.TimeFrame
            };

            var secondTickerQuotes = await _quotesRepository.GetAllQuotes(secondTickerQuotesRequest);
            secondTickerQuotes.RequiredNotNull(nameof(secondTickerQuotes), secondTickerQuotesRequest);

            if (secondTickerQuotes.Any() == false)
            {
                return new CompareQuotesResponse()
                {
                    IsSuccess = false,
                    Message = "Не найдено котировок для второго инструмента"
                };
            }

            var firstComparedDate = new DateTime(Math.Max(firstTickerQuotes.Min(x => x.Date).Ticks, secondTickerQuotes.Min(x => x.Date).Ticks));
            var lastComparedDate = new DateTime(Math.Min(firstTickerQuotes.Max(x => x.Date).Ticks, secondTickerQuotes.Max(x => x.Date).Ticks));

            firstTickerQuotes = firstTickerQuotes.Where(x => x.Date >= firstComparedDate && x.Date <= lastComparedDate).ToList();
            secondTickerQuotes = secondTickerQuotes.Where(x => x.Date >= firstComparedDate && x.Date <= lastComparedDate).ToList();

            var onlyInFirstTicker = firstTickerQuotes.Select(x => x.Date).Except(secondTickerQuotes.Select(x => x.Date)).ToList();
            var onlyInSecondTicker = secondTickerQuotes.Select(x => x.Date).Except(firstTickerQuotes.Select(x => x.Date)).ToList();

            var secondTickerQuotesDict = secondTickerQuotes.ToDictionary(x => x.Date);

            var differences = new List<string>();

            var dateDiffs = new List<string>();

            foreach (var quote in firstTickerQuotes)
            {
                if (secondTickerQuotesDict.TryGetValue(quote.Date, out var secondTickerQuote))
                {
                    dateDiffs.Clear();

                    if (IsDifferent(quote.Open ?? -1, secondTickerQuote.Open ?? -1, request.MaxDifferencePercent, nameof(quote.Open), out var diff))
                    {
                        dateDiffs.Add(diff);
                    }

                    if (IsDifferent(quote.Hi, secondTickerQuote.Hi, request.MaxDifferencePercent, nameof(quote.Hi), out diff))
                    {
                        dateDiffs.Add(diff);
                    }

                    if (IsDifferent(quote.Low, secondTickerQuote.Low, request.MaxDifferencePercent, nameof(quote.Low), out diff))
                    {
                        dateDiffs.Add(diff);
                    }

                    if (IsDifferent(quote.Close ?? -1, secondTickerQuote.Close ?? -1, request.MaxDifferencePercent, nameof(quote.Close), out diff))
                    {
                        dateDiffs.Add(diff);
                    }

                    if (dateDiffs.Count > 0)
                    {
                        differences.Add($"{Auxiliary.GetDateString(quote.Date, request.TimeFrame)}: {string.Join(';', dateDiffs)}");
                    }
                }
            }

            return new CompareQuotesResponse()
            {
                IsSuccess = true,
                FirstComparedDate = Auxiliary.GetDateString(firstComparedDate, request.TimeFrame),
                LastComparedDate = Auxiliary.GetDateString(lastComparedDate, request.TimeFrame),
                OnlyInFirstTicker = onlyInFirstTicker.Select(x => Auxiliary.GetDateString(x, request.TimeFrame)).ToList(),
                OnlyInSecondTicker = onlyInSecondTicker.Select(x => Auxiliary.GetDateString(x, request.TimeFrame)).ToList(),
                Differences = differences
            };
        }

        private static bool IsDifferent(decimal one, decimal two, int maxDifferencePercent, string name, out string res)
        {
            var val1 = Auxiliary.GetDecimalString(one);
            var val2 = Auxiliary.GetDecimalString(two);

            if (val1 != val2 && Math.Abs(one - two) > Math.Min(one, two) * maxDifferencePercent / 100)
            {
                res = $"{name}:{val1} - {val2}";
                return true;
            }
            else
            {
                res = string.Empty;
                return false;
            }
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

            return QuotesService.BL.Static.AuxiliaryBL.IsReallyLastQuote(request.Quotes.Max(x => x.Date), request.TickerMarketTimeFrame.TimeFrame);
        }
    }
}
