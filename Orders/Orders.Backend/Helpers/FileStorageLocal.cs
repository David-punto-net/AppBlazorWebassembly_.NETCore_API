using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Orders.Backend.Helpers;

public class FileStorageLocal : IFileStorage
{
    private readonly Cloudinary _cloudinary;

    public FileStorageLocal(IConfiguration configuration)
    {
        var cloudinaryConfig = configuration.GetSection("Cloudinary");
        var account = new Account(
            cloudinaryConfig["CloudName"],
            cloudinaryConfig["ApiKey"],
            cloudinaryConfig["ApiSecret"]
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task RemoveFileAsync(string path, string containerName)
    {

        var publicId = Path.Combine(containerName,Path.GetFileNameWithoutExtension(path)).Replace("\\", "/");
        var deletionParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deletionParams);

    }

    public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
    {
        using (var stream = new MemoryStream(content))
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                AssetFolder = containerName,
                PublicId = Path.Combine(containerName, Path.GetFileNameWithoutExtension(fileName)).Replace("\\", "/")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }
    }
}


/*
  private readonly string _localStoragePath;

    public FileStorageLocal(IConfiguration configuration)
    {
        _localStoragePath = configuration.GetConnectionString("LocalStoragePath")!;
    }

    public async Task RemoveFileAsync(string path, string containerName)
    {
        var directoryPath = Path.Combine(_localStoragePath, containerName);
        var filePath = Path.Combine(directoryPath, Path.GetFileName(path));

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
    {
        var directoryPath = Path.Combine(_localStoragePath, containerName);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(directoryPath, fileName);

        await File.WriteAllBytesAsync(filePath, content);

        return filePath.ToString();
    }
*/