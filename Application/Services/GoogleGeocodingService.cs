using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application.Services
{
    public class GoogleGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public GoogleGeocodingService(string apiKey, HttpClient? httpClient = null)
        {
            _apiKey = apiKey;
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<GeocodeResult?> GeocodeAsync(string address, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(_apiKey))
                return null;

            var encoded = UrlEncoder.Default.Encode(address);
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encoded}&key={_apiKey}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<GeocodeResponse>(stream, JsonOptions, cancellationToken);
            var first = result?.Results?.FirstOrDefault();
            if (first?.Geometry?.Location == null)
                return null;

            var county = first.AddressComponents?
                .FirstOrDefault(c => c.Types != null && c.Types.Contains("administrative_area_level_2"))?
                .LongName;

            return new GeocodeResult
            {
                Lat = first.Geometry.Location.Lat,
                Lng = first.Geometry.Location.Lng,
                County = county
            };
        }

        private sealed class GeocodeResponse
        {
            public GeocodeResultItem[]? Results { get; set; }
        }

        private sealed class GeocodeResultItem
        {
            public GeocodeGeometry? Geometry { get; set; }
            public GeocodeAddressComponent[]? AddressComponents { get; set; }
        }

        private sealed class GeocodeGeometry
        {
            public GeocodeLocation? Location { get; set; }
        }

        private sealed class GeocodeLocation
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        private sealed class GeocodeAddressComponent
        {
            public string? LongName { get; set; }
            public string[]? Types { get; set; }
        }
    }

    public sealed class GeocodeResult
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string? County { get; set; }
    }
}
