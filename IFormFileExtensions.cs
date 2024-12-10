public static class IFormFileExtensions
{
    private readonly static string[] _allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
    private readonly static int _fileSizeLimit = 5 * 1024 * 1024;
    private readonly static string[] _allowedMimeTypes = new string[] { "image/jpeg", "image/png", "image/gif" };

    public static bool IsValidImage(this IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return false;
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension) && file.Length <= _fileSizeLimit && _allowedMimeTypes.Contains(file.ContentType);
    }
    public static async Task<byte[]> ToByteArrayAsync(this IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}