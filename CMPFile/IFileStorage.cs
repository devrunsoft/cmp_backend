
namespace CMPFile
{
    public interface IFileStorage
    {
        Task<string> UploadAsync(string objectName, Stream data, string? contentType = null, CancellationToken ct = default);
        Task<Stream> DownloadAsync(string objectName, CancellationToken ct = default);
        Task<bool> ExistsAsync(string objectName, CancellationToken ct = default);
        Task DeleteAsync(string objectName, CancellationToken ct = default);

        // Optional convenience:
        Task<string> GetPresignedGetUrlAsync(string objectName, TimeSpan expires, CancellationToken ct = default);
        Task<string> GetPresignedPutUrlAsync(string objectName, TimeSpan expires, string? contentType = null, CancellationToken ct = default);
    }
}