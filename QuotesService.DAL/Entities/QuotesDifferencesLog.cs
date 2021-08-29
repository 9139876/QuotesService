using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Entities
{
    [Table("QuotesDifferencesLog")]
    public class QuotesDifferencesLog
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TickerId { get; set; }


    }
}
