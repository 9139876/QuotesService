using System.ComponentModel.DataAnnotations;

namespace QuotesService.BL.Models
{
    public class NasdaqGetDataInfoModel
    {
        [Required]
        public string Class { get; set; }

        [Required]
        public string Symbol { get; set; }
    }
}
