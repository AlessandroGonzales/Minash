using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName, string webRootPath);
        Task<List<string>> UploadFilesAsync(List<IFormFile> files, string folderName, string webRootPath);
    }
}
