using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuotesService.Api.Enum;

namespace QuotesService.DAL.Entities
{
    [Table("Markets")]
    public class MarketEntity
    {
        [Key]
        public int Id { get; set; }

        public QuotesProviderEnum QuotesProvider { get; set; }

        public string Name { get; set; }
    }
}
