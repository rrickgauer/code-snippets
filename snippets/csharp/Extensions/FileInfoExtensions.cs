public static class FileInfoExtensions
{
    public static async Task<byte[]> GetBytesAsync(this FileInfo fileInfo)
    {
        return await File.ReadAllBytesAsync(fileInfo.FullName);
    }
}