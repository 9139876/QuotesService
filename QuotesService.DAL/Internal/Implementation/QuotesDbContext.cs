using CommonLibraries.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using QuotesService.DAL.Entities;

namespace QuotesService.DAL.Internal.Implementation
{
    internal class QuotesDbContext : BaseDbContext, IQuotesDbContext
    {
        public QuotesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MarketEntity> Markets { get; set; }
        public DbSet<QuoteEntity> Quotes { get; set; }
        public DbSet<QuotesProviderEntity> QuotesProviders { get; set; }
        public DbSet<TickerEntity> Tickers { get; set; }
        public DbSet<TickerTFEntity> TickerTFs { get; set; }
        public DbSet<QuotesProviderTFEntity> QuotesProviderTFs { get; set; }
        public DbSet<QuotesProviderTaskEntity> QuotesProvidersTasks { get; set; }
    }
}
