using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuotesService.DAL.Entities
{
    [Table("Quotes")]
    public class QuoteEntity
    {
        [Key]
        public int Id { get; set; }

        public int ParentTickerTFId { get; set; }

        public DateTime Date { get; set; }

        public decimal? Open { get; set; }

        public decimal Hi { get; set; }

        public decimal Low { get; set; }

        public decimal? Close { get; set; }

        public decimal? Volume { get; set; }
    }
}
