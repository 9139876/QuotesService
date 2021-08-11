using CommonLibraries.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using QuotesService.BL.Services;
using QuotesService.WebApp.Models;
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
        private readonly IFormatProvider _formatProvider;
        private readonly IFileLoaderService _fileLoaderService;

        public LoadFromFileApiController(
            IFileLoaderService fileLoaderService)
        {
            _fileLoaderService = fileLoaderService;
            _formatProvider = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
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

            if (string.IsNullOrEmpty(request.Text))
            {
                return new TryParseDateResponse()
                {
                    Success = false,
                    Error = $"{nameof(request.Text)} is empty."
                };
            }

            if (string.IsNullOrEmpty(request.Format))
            {
                return new TryParseDateResponse()
                {
                    Success = false,
                    Error = $"{nameof(request.Format)} is empty."
                };
            }

            var success = DateTime.TryParseExact(request.Text, request.Format, _formatProvider, DateTimeStyles.AllowWhiteSpaces, out var dateTime);

            if (!success)
            {
                return new TryParseDateResponse()
                {
                    Success = false,
                    Error = "Не удалось разобрать дату-время"
                };
            }

            return new TryParseDateResponse()
            {
                Success = true,
                Year = dateTime.Year,
                Month = dateTime.Month,
                Day = dateTime.Day,
                Hour = dateTime.Hour,
                Min = dateTime.Minute,
                Sec = dateTime.Second
            };
        }
    }
}
