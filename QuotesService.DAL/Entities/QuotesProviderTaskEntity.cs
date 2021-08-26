using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Entities
{
    [Table("QuotesProvidersTasks")]
    public class QuotesProviderTaskEntity
    {
        [Key]
        public int Id { get; set; }

        public int TickerTFId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatePeriodInSecond { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
