using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Entities;

namespace QuotesService.DAL.Internal.Implementation
{
    internal class MarketsDbContext : BaseDbContext, IMarketsDbContext
    {
        public MarketsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MarketEntity> MarketsEntities { get; set; }
        public DbSet<QuoteEntity> QuoteEntity { get; set; }
        public DbSet<QuotesProviderEntity> QuotesProviderEntity { get; set; }
        public DbSet<TickerInfoEntity> TickerInfoEntity { get; set; }
        public DbSet<TickersTimeFramesEntity> TickerTFEntity { get; set; }
    }
}
