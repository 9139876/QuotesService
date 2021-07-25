using CommonLibraries.EF;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Entities;

namespace QuotesService.DAL.Internal
{
    public interface IMarketsDbContext : IBaseDbContext
    {
        DbSet<MarketEntity> MarketsEntities { get; set; }

        DbSet<QuoteEntity> QuoteEntity { get; set; }

        DbSet<QuotesProviderEntity> QuotesProviderEntity { get; set; }

        DbSet<TickerInfoEntity> TickerInfoEntity { get; set; }

        DbSet<TickersTimeFramesEntity> TickerTFEntity { get; set; }
    }
}
