using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuotesService.Api.Enum;

namespace QuotesService.DAL.Entities
{
    [Table("TickersTFs")]
    public class TickersTimeFramesEntity
    {
        [Key]
        public int Id { get; set; }

        public int TickerInfoId { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }

        //public string GetUrl { get; set; }

        //public bool NeedUpdate { get; set; }

        //public DateTime? NextUpdateDate { get; set; }
    }
}
