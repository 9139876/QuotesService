using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.BL.Services.Implementation
{
    internal class FileLoaderService : IFileLoaderService
    {
        private readonly string _filePath;

        public FileLoaderService(
            IConfiguration configuration)
        {
            _filePath = configuration.GetSection("FilePath")?.Value ?? throw new InvalidOperationException("File path not defined in config.");
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
