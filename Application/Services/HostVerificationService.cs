using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Models;
using CMPNatural.Core.Services;
using Microsoft.Extensions.Configuration;

namespace CMPNatural.Application.Services
{
    public class HostVerificationService : IHostVerificationService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly string _verificationSecret;

        public HostVerificationService(IConfiguration configuration)
        {
            _verificationSecret = configuration["JWT:Secret"] ?? configuration["AppSetting:host"] ?? "cmpnatural-host-verification";
        }

        public string CreateSignature(string challenge)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_verificationSecret));
            var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(challenge));
            return Convert.ToHexString(bytes);
        }

        public async Task<bool> VerifyHostAsync(string? host, CancellationToken cancellationToken = default)
        {
            var normalizedHost = NormalizeHost(host);
            if (string.IsNullOrWhiteSpace(normalizedHost))
            {
                return false;
            }

            var challenge = Guid.NewGuid().ToString("N");
            var expectedSignature = CreateSignature(challenge);

            return await TryVerifyAsync("https", normalizedHost, challenge, expectedSignature, cancellationToken) ||
                   await TryVerifyAsync("http", normalizedHost, challenge, expectedSignature, cancellationToken);
        }

        private static string? NormalizeHost(string? host)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                return null;
            }

            var trimmed = host.Trim();
            if (Uri.TryCreate(trimmed, UriKind.Absolute, out var absoluteUri))
            {
                return absoluteUri.Host.ToLowerInvariant();
            }

            var sanitized = trimmed
                .Replace("https://", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("http://", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Trim('/')
                .ToLowerInvariant();

            var slashIndex = sanitized.IndexOf('/');
            return slashIndex >= 0 ? sanitized[..slashIndex] : sanitized;
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };

            return new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        private static async Task<HostVerificationResponse?> GetVerificationResponseAsync(string url, CancellationToken cancellationToken)
        {
            using var client = CreateHttpClient();
            using var response = await client.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<HostVerificationResponse>(stream, JsonOptions, cancellationToken);
        }

        private static bool IsValidResponse(HostVerificationResponse? response, string normalizedHost, string expectedSignature)
        {
            return response != null &&
                   string.Equals(response.Host, normalizedHost, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(response.Signature, expectedSignature, StringComparison.OrdinalIgnoreCase);
        }

        private async Task<bool> TryVerifyAsync(
            string scheme,
            string normalizedHost,
            string challenge,
            string expectedSignature,
            CancellationToken cancellationToken)
        {
            try
            {
                var url = $"{scheme}://{normalizedHost}/api/public/HostVerification?challenge={Uri.EscapeDataString(challenge)}";
                var response = await GetVerificationResponseAsync(url, cancellationToken);
                return IsValidResponse(response, normalizedHost, expectedSignature);
            }
            catch
            {
                return false;
            }
        }
    }
}
