using QuotesService.Api.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuotesService.DAL.Entities
{
    [Table("QuotesProviders")]
    public class QuotesProviderEntity
    {
        [Key]
        public int Id { get; set; }

        public QuotesProviderEnum QuotesProviderType { get; set; }

        public string Name { get; set; }
    }
}
