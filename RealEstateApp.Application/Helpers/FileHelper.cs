using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Domain.Entities;
using System.Text.RegularExpressions;

namespace RealEstateApp.Application.Helpers;

public static class FileHelper
{
    private const string ContainerName = "imagenes";
    private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=appimagenes;AccountKey=ORchHcBlJyCbp3iara78fP38RJdna+b9h9BTBjqpushi070UsV9Z2DU7Huic46ogVjloFdmTRwOH+AStZ91Skw==;EndpointSuffix=core.windows.net";
    
    public static async Task<List<PropertyImage>> SaveImagesAsync(IEnumerable<IFormFile> images)
    {
        var propertyImages = new List<PropertyImage>();
        var blobServiceClient = new BlobServiceClient(ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        foreach (var image in images)
        {
            var sanitizedFileName = SanitizeFileName(image.FileName);
            var fileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = image.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            propertyImages.Add(new PropertyImage
            {
                ImageUrl = blobClient.Uri.ToString()
            });
        }

        return propertyImages;
    }

    public static async Task<string> SaveImageAsync(IFormFile image)
    {
        var blobServiceClient = new BlobServiceClient(ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        var sanitizedFileName = SanitizeFileName(image.FileName);
        var fileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
        var blobClient = containerClient.GetBlobClient(fileName);

        using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return blobClient.Uri.ToString();
    }
    
    public static async Task DeleteImageAsync(string imagePath)
    {
        var blobServiceClient = new BlobServiceClient(ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        var blobName = Path.GetFileName(new Uri(imagePath).LocalPath);
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();
    }


    private static string SanitizeFileName(string fileName)
    {
        return Regex.Replace(fileName, @"[^a-zA-Z0-9_\-\.]", "_");
    }
}
