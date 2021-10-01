using System.ComponentModel.DataAnnotations;

namespace QuotesService.BL.Models
{
    public class YahooFinanceGetDataInfoModel
    {
        [Required]
        public string Symbol { get; set; }
    }
}
