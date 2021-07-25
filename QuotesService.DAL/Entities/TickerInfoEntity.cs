using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuotesService.Api.Enum;

namespace QuotesService.DAL.Entities
{
    [Table("TickersInfos")]
    public class TickerInfoEntity
    {
        [Key]
        public int Id { get; set; }

        public int MarketId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TickerTypeEnum Type { get; set; }

        public string Currency { get; set; }

        public string Symbol { get; set; }

        public string VolumeCode { get; set; }
    }
}
