using System.ComponentModel.DataAnnotations;

namespace QuotesService.BL.Models
{
    public class StooqGetDataInfoModel
    {
        [Required]
        public string Symbol { get; set; }
    }
}
