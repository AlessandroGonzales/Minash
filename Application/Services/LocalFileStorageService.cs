using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Application.Services
{
    public class LocalFileStorageService : IFileStorageService
    {

        private readonly IHostEnvironment _env;
        public LocalFileStorageService(IHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName, string webRootPath)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("El archivo es nulo o está vacío.");
            }

            // Generar nombre de archivo único
            string extension = Path.GetExtension(file.FileName);
            string uniqueFileName = Guid.NewGuid().ToString() + extension;

            // La ruta absoluta donde se guardará el archivo
            string uploadsFolder = Path.Combine(webRootPath, "images", folderName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string relativeUrl = $"/images/{folderName}/{uniqueFileName}";

            string simulatedBaseUrl = "https://minash.local";

            return simulatedBaseUrl + relativeUrl;
        }
    }

}

