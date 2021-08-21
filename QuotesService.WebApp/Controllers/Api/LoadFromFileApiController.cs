using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.WebApp.Models;
using QuotesService.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Controllers.Api
{
    [Route("/load-from-file-api")]
    public class LoadFromFileApiController : Controller
    {

        private readonly IQuotesFileLoaderService _fileLoaderService;

        public LoadFromFileApiController(
            IQuotesFileLoaderService fileLoaderService)
        {
            _fileLoaderService = fileLoaderService;
        }

        [HttpGet("get-file-text")]
        public async Task<string> GetFileText(string fileName)
        {
            return await _fileLoaderService.GetFileTextAsync(fileName);
        }

        [HttpPost("date-time-try-parse")]
        public TryParseDateResponse DateTimeTryParse([FromBody] TryParseDateRequest request)
        {
            request.RequiredNotNull(nameof(request));

            return _fileLoaderService.DateTimeTryParse(request);
        }
    }
}
