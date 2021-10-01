using System.ComponentModel.DataAnnotations;

namespace QuotesService.BL.Models
{
    public class AlphaVantageGetDataInfoModel
    {
        [Required]
        public string FunctionPrefix { get; set; }

        public string Symbol { get; set; }

        public string FromSymbol { get; set; }

        public string ToSymbol { get; set; }

        public string Market { get; set; }
    }
}
