using System.Text.RegularExpressions;
using CMPFile;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace CMPFile;

public sealed class MinioFileStorage : IFileStorage
{
    string bucket = DateTime.UtcNow.ToString("yyyy-MM");
    private readonly IMinioClient _client;
    private readonly MinioOptions _opt;
    private static readonly FileExtensionContentTypeProvider _types = new();

    public MinioFileStorage(IOptions<MinioOptions> options)
    {
        _opt = options.Value;

        var builder = new MinioClient()
            .WithEndpoint(_opt.Endpoint.Replace("https://", "").Replace("http://", ""))
            .WithCredentials(_opt.AccessKey, _opt.SecretKey);

        if (_opt.WithSSL) builder = builder.WithSSL();

        _client = builder.Build();
    }

    private async Task EnsureBucketAsync(string bucketName, CancellationToken ct)
    {
        if (!_opt.AutoCreateBucket) return;

        var exists = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct);
        if (!exists)
            await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), ct);
    }

    public async Task<string> UploadAsync(string objectName, Stream data, string? contentType = null, CancellationToken ct = default)
    {
        await EnsureBucketAsync(bucket, ct);

        if (contentType is null && _types.TryGetContentType(objectName, out var ctGuess))
            contentType = ctGuess ?? "application/octet-stream";
        contentType ??= "application/octet-stream";

        // MinIO PutObject requires a known size; if not seekable, buffer to memory/file.
        long size;
        if (data.CanSeek)
        {
            size = data.Length - data.Position;
        }
        else
        {
            var ms = new MemoryStream();
            await data.CopyToAsync(ms, ct);
            ms.Position = 0;
            data = ms;
            size = ms.Length;
        }
        var url = $"{bucket}/{objectName}";
        var args = new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(url)
            .WithStreamData(data)
            .WithObjectSize(size)
            .WithContentType(contentType);

        await _client.PutObjectAsync(args, ct);
        return url;
    }

    public async Task<Stream?> DownloadAsync(string objectName, CancellationToken ct = default)
    {

            try
            {
                // found → download
                var ms = new MemoryStream();
                var getArgs = new GetObjectArgs()
                    .WithBucket(objectName.ToBucket())
                    .WithObject(objectName)
                    .WithCallbackStream(stream => stream.CopyTo(ms));
                await _client.GetObjectAsync(getArgs, ct);
                ms.Position = 0;
                return ms;
            }
            catch (Minio.Exceptions.ObjectNotFoundException)
            {
                // not in this bucket → continue
            }
            catch
            {
                // other errors → rethrow or log
            }

        return null; 
    }

    public async Task<bool> ExistsAsync(string objectName, CancellationToken ct = default)
    {
        try
        {
            var args = new StatObjectArgs().WithBucket(objectName.ToBucket()).WithObject(objectName);
            await _client.StatObjectAsync(args, ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task DeleteAsync(string objectName, CancellationToken ct = default)
    {
        var args = new RemoveObjectArgs().WithBucket(objectName.ToBucket()).WithObject(objectName);
        await _client.RemoveObjectAsync(args, ct);
    }

    public async Task<string> GetPresignedGetUrlAsync(string objectName, TimeSpan expires, CancellationToken ct = default)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(objectName.ToBucket())
            .WithObject(objectName)
            .WithExpiry((int)expires.TotalSeconds);
        return await _client.PresignedGetObjectAsync(args);
    }

    public async Task<string> GetPresignedPutUrlAsync(string objectName, TimeSpan expires, string? contentType = null, CancellationToken ct = default)
    {
        var args = new PresignedPutObjectArgs()
            .WithBucket(objectName.ToBucket())
            .WithObject(objectName)
            .WithExpiry((int)expires.TotalSeconds);
        if (!string.IsNullOrWhiteSpace(contentType))
            args = args.WithHeaders(new Dictionary<string, string> { ["Content-Type"] = contentType });
        return await _client.PresignedPutObjectAsync(args);
    }


}
/// <summary>
/// Proper extension that extracts (bucket, objectName) from a path.
/// If the first segment matches yyyy-MM, it is used as the bucket; otherwise,
/// current UTC month (yyyy-MM) is used as the bucket.
/// </summary>
public static class BucketPathExtensions
{
    private static readonly Regex MonthBucketRegex = new(@"^\d{4}-\d{2}$", RegexOptions.Compiled);

    public static string ToBucket(this string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("filePath is required.", nameof(filePath));

        var segments = filePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length > 1 && MonthBucketRegex.IsMatch(segments[0]))
        {
            var bucket = segments[0];
            var objectName = string.Join('/', segments.Skip(1));
            return bucket;
        }
        else
        {
            var bucket = DateTime.UtcNow.ToString("yyyy-MM");
            var objectName = string.Join('/', segments);
            return bucket;
        }
    }
}