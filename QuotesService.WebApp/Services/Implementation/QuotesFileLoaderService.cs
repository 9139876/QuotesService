using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuotesService.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Services.Implementation
{
    internal class QuotesFileLoaderService : IQuotesFileLoaderService
    {
        private readonly IFormatProvider _formatProvider;
        private readonly string _filePath;

        public QuotesFileLoaderService(
            IConfiguration configuration)
        {
            _filePath = configuration.GetSection("FilePath")?.Value ?? throw new InvalidOperationException("File path not defined in config.");
            _formatProvider = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
        }

        public TryParseDateResponse DateTimeTryParse([FromBody] TryParseDateRequest request)
        {
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

        public async Task<string> GetFileTextAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "File name is empty.";
            }

            var fullFileName = Path.Combine(_filePath, fileName);

            if (File.Exists(fullFileName) == false)
            {
                return $"File {fileName} is not exist in {_filePath}";
            }

            return await ReadFileTextAsync(fullFileName);
        }

        private async Task<string> ReadFileTextAsync(string fullFileName)
        {
            using (var reader = new StreamReader(fullFileName))
            {
                var result = await reader.ReadToEndAsync();
                return result;
            }
        }
    }
}
