using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Core.Base;

namespace QuotesService.Api.Models.RequestResponse
{
    public class CheckGetQuotesResponse
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime StartDate { get; set; }
    }
}
