using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using CMPNatural.Application.Responses;

public class GoogleDirectionsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public GoogleDirectionsService(string apiKey)
    {
        _httpClient = new HttpClient();
        _apiKey = apiKey;
    }

    public async Task<DirectionsResult> GetDirectionsAsync(DirectionCommand command)
    {
        string origin = $"{command.Origin.Lat},{command.Origin.Lng}";
        string destination = $"{command.Destination.Lat},{command.Destination.Lng}";
        string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&key={_apiKey}";

        if (command.Waypoints is { Count: > 0 })
        {
            var waypointsStr = string.Join("|", command.Waypoints.Select(w => $"{w.Lat},{w.Lng}"));
            // URL-encode waypoints just in case
            var encoded = UrlEncoder.Default.Encode(waypointsStr);
            url += $"&waypoints={encoded}";
        }

        using var resp = await _httpClient.GetAsync(url);
        resp.EnsureSuccessStatusCode();

        var stream = await resp.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<DirectionsResult>(stream, JsonOptions);

        return result ?? new DirectionsResult();
    }
}