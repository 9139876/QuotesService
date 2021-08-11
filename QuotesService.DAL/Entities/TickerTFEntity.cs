using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuotesService.Api.Enum;

namespace QuotesService.DAL.Entities
{
    [Table("TickersTFs")]
    public class TickerTFEntity
    {
        [Key]
        public int Id { get; set; }

        public int TickerId { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }
    }
}
