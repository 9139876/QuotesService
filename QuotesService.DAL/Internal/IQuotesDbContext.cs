using CommonLibraries.EF;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Entities;

namespace QuotesService.DAL.Internal
{
    public interface IQuotesDbContext : IBaseDbContext
    {
        DbSet<MarketEntity> Markets { get; set; }

        DbSet<QuotesProviderTFEntity> QuotesProviderTFs { get; set; }

        DbSet<QuotesProviderTaskEntity> QuotesProvidersTasks { get; set; }

        DbSet<QuoteEntity> Quotes { get; set; }

        DbSet<QuotesProviderEntity> QuotesProviders { get; set; }

        DbSet<TickerEntity> Tickers { get; set; }

        DbSet<TickerInfoEntity> TickersInfoes { get; set; }

        DbSet<TickerInfoNameEntity> TickersInfoesNames { get; set; }

        DbSet<TickerTFEntity> TickerTFs { get; set; }
    }
}
