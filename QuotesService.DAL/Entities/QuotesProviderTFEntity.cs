using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotesService.Api.Enum;

namespace QuotesService.DAL.Entities
{
    /// <summary>
    /// Поддерживаемые поставщиком котировок таймфреймы
    /// </summary>
    [Table("QuotesProvidersTFs")]
    public class QuotesProviderTFEntity
    {
        [Key]
        public int Id { get; set; }

        public int QuotesProviderId { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }
    }
}
