using System.Text;

namespace QuotesService.Api.Models.RequestResponse
{
    public class QuotesInfo
    {
        public string TimeFrameName { get; set; }

        public int QuotesCount { get; set; }

        public string FirstDate { get; set; }

        public string LastDate { get; set; }

        public string MinPrice { get; set; }

        public string MinPriceDate { get; set; }

        public string MaxPrice { get; set; }

        public string MaxPriceDate { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(200);
            sb.AppendLine($"Таймфрейм: {TimeFrameName}");
            sb.AppendLine($"Всего котировок: {QuotesCount}");
            sb.AppendLine($"Дата первой котировки: {FirstDate}");
            sb.AppendLine($"Дата последней котировки: {LastDate}");
            sb.AppendLine($"Минимальная цена: {MinPrice} от {MinPriceDate}");
            sb.AppendLine($"Максимальная цена: {MaxPrice} от {MaxPriceDate}");

            return sb.ToString();
        }
    }
}
