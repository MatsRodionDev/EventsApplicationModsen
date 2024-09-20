using EventsApplication.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EventsApplication.Infrastructure.Services
{
    public class FileService : IFileService
    {
        IWebHostEnvironment _environment;

        public FileService(
            IWebHostEnvironment environment)
        { 
            _environment = environment;
        }


        public async Task<string> SaveFilesync(IFormFile file, Guid eventId, CancellationToken cancellationToken)
        {
            var webRootPath = _environment.WebRootPath;

            var path = Path.Combine(webRootPath, "Uploads");

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var extenssion = Path.GetExtension(file.FileName);

            var fileName = $"{eventId}_{extenssion}";

            var fileNameWithPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fileNameWithPath, FileMode.Create);

            await file.CopyToAsync(stream, cancellationToken);

            return fileName;
        }

        public void Delete(string fileName)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "Uploads", fileName);

            if (File.Exists(filePath))
            { 
                File.Delete(filePath);
            }
        }

    }
}
