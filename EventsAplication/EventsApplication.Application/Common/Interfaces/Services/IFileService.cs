using Microsoft.AspNetCore.Http;

namespace EventsApplication.Application.Common.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> SaveFilesync(IFormFile file, Guid eventId, CancellationToken cancellationToken);
        void Delete(string fileName);
    }
}
