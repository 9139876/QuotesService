using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuotesService.Api.Enum;

namespace QuotesService.DAL.Entities
{
    [Table("Tickers")]
    public class TickerEntity
    {
        [Key]
        public int Id { get; set; }

        public int MarketId { get; set; }

        public int? QuotesProviderId { get; set; }

        public string ProviderGetDataInfo { get; set; }

        public string Name { get; set; }
    }
}
