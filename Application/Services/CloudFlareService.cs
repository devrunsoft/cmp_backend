using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;

namespace CMPNatural.Application.Services
{
    public class CloudflareDnsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly bool isProduction;
        const string ENVIRONMENT_DEVELOPMENT = "development";

        public CloudflareDnsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            isProduction = (!environmentName.Equals(ENVIRONMENT_DEVELOPMENT, StringComparison.OrdinalIgnoreCase));
        }


        public async Task<bool> CreateTenantWildcardDnsAsync(string tenant, string serverIp)
        {
            if (!isProduction)
            {
                return false;
            }

            if (await DnsRecordExistsAsync(tenant))
            {
                return false;
            }

            var token = _configuration["Cloudflare:ApiToken"];
            var zoneId = _configuration["Cloudflare:ZoneId"];

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://api.cloudflare.com/client/v4/zones/{zoneId}/dns_records"
            );

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                type = "A",
                name = GetFullRecordName(tenant),
                content = serverIp,
                ttl = 1,
                proxied = true
            });

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Cloudflare DNS creation failed: {error}");
            }

            return true;
        }

        public async Task<bool> DnsRecordExistsAsync(string tenant)
        {
            var records = await GetDnsRecordsAsync(tenant);
            return records.GetArrayLength() > 0;
        }

        public async Task DeleteDnsRecordIfExistsAsync(string tenant)
        {
            if (!isProduction)
            {
                return;
            }
            var token = _configuration["Cloudflare:ApiToken"];
            var zoneId = _configuration["Cloudflare:ZoneId"];
            var records = await GetDnsRecordsAsync(tenant);

            foreach (var recordId in records
                .EnumerateArray()
                .Select(x => x.TryGetProperty("id", out var idProperty) ? idProperty.GetString() : null)
                .Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Delete,
                    $"https://api.cloudflare.com/client/v4/zones/{zoneId}/dns_records/{recordId}");

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Cloudflare DNS delete failed: {error}");
                }
            }
        }

        private string GetFullRecordName(string tenant)
        {
            var domain = _configuration["Cloudflare:Domain"];
            return $"{tenant}.{domain}";
        }

        private async Task<JsonElement> GetDnsRecordsAsync(string tenant)
        {
            var token = _configuration["Cloudflare:ApiToken"];
            var zoneId = _configuration["Cloudflare:ZoneId"];
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.cloudflare.com/client/v4/zones/{zoneId}/dns_records?name={Uri.EscapeDataString(GetFullRecordName(tenant))}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);
            return document.RootElement.GetProperty("result").Clone();
        }
    }
}
