namespace Orders.Backend.Helpers;

public class FileStorageLocal : IFileStorage
{
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
}