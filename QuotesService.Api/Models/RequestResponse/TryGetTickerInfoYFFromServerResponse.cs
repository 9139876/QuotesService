using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Core.Base;
using QuotesService.Api.Models.RequestResponse.Interfaces;

namespace QuotesService.Api.Models.RequestResponse
{
    public class TryGetTickerInfoYFFromServerResponse : BaseGuidResponse, ITryGetTickerInfoFromServerResponse
    {
        public string Data { get; set; }

        public TickerInfoModel TickerInfo { get; set; }
    }
}
