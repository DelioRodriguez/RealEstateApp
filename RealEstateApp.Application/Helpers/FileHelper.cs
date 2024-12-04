using Microsoft.AspNetCore.Http;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Helpers;

public static class FileHelper
{
    public static async Task<List<PropertyImage>> SaveImagesAsync(IEnumerable<IFormFile> images, string folderPath)
    {
        var propertyImages = new List<PropertyImage>();
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath);
        
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        foreach (var image in images)
        {
            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadPath, fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            
            propertyImages.Add(new PropertyImage
            {
                ImageUrl = $"/{folderPath}/{fileName}"
            });
        }

        return propertyImages;
    }
    public static async Task<string> SaveImageAsync(IFormFile image, string folderPath)
    {
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath);

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
        var filePath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }
        
        return $"/{folderPath}/{fileName}";
    }

    public static void DeleteImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            throw new ArgumentException("El path de la imagen no puede ser nulo o vacío.");

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}