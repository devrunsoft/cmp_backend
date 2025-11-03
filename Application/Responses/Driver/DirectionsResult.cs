using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace CMPNatural.Application.Responses
{
    public class DirectionsResult
    {
        [JsonPropertyName("routes")]
        public List<RouteResponce> routes { get; set; } = new();
    }

    public class RouteResponce
    {
        [JsonPropertyName("legs")]
        public List<Leg> legs { get; set; } = new();

        // maps overview_polyline → OverviewPolyline
        [JsonPropertyName("overview_polyline")]
        public OverviewPolyline overview_polyline { get; set; } = new();
    }

    public class Leg
    {
        [JsonPropertyName("steps")]
        public List<Step> steps { get; set; } = new();
    }

    public class Step
    {
        [JsonPropertyName("polyline")]
        public Polyline polyline { get; set; } = new();
    }

    public class OverviewPolyline
    {
        [JsonPropertyName("points")]
        public string points { get; set; } = "";
    }

    public class Polyline
    {
        [JsonPropertyName("points")]
        public string points { get; set; } = "";
    }
}