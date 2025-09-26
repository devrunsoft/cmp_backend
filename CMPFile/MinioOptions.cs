using System;
namespace CMPFile
{
    public sealed class MinioOptions
    {
        public required string Endpoint { get; init; }    // e.g. "http://localhost:9000"
        public required string AccessKey { get; init; }   // MINIO_ROOT_USER or a service user
        public required string SecretKey { get; init; }   // MINIO_ROOT_PASSWORD or a service user secret
        public bool WithSSL { get; init; } = false;       // true if using https
        public string? Region { get; init; }              // optional
        public bool AutoCreateBucket { get; init; } = true;
    }
}

