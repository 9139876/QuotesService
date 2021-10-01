using QuotesService.Api.Enum;

namespace QuotesService.BL.Models
{
    public class QuotesProvider
    {
        public QuotesProviderTypeEnum QuotesProviderType { get; set; }

        public string QuotesProviderName { get; set; }
    }
}
