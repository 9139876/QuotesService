using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Core.Base;
using QuotesService.Api.Enum;

namespace QuotesService.Api.Models.RequestResponse
{
    public class GetQuotesRequest : BaseGuidRequest
    {
        public string Symbol { get; set; }

        public TimeFrameEnum TimeFrame { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate
        {
            get => _endDate ?? DateTime.Now;
            set => _endDate = value;
        }

        private DateTime? _endDate;
    }
}
