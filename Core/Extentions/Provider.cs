using GeoCoordinatePortable;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;

namespace CMPNatural.Core.Entities
{
    public partial class Provider
    {
        private const double MetersPerMile = 1609.344;

        private static readonly GeoJsonReader _geoJsonReader = new();

        private bool IsDistanceIn(double sLatitude, double sLongitude, ServiceArea model)
        {
            if (Lat == null || Long == null) return false;

            var sCoord = new GeoCoordinate(sLatitude, sLongitude);
            var eCoord = new GeoCoordinate(model.Lat!.Value, model.Lng!.Value);
            var dis = sCoord.GetDistanceTo(eCoord);

            return dis <= (model.Radius * MetersPerMile);
        }

        public bool IsPointInCity(double lat, double lng)
        {
            var point = new Point(lng, lat);

            foreach (var item in ServiceArea)
            {
                if (item.ServiceAreaType == Enums.ServiceAreaTypeEnum.Circle)
                {
                    if (IsDistanceIn(lat, lng, item))
                        return true;
                }
                else
                {
                    try
                    {
                        var geoJson = JObject.Parse(item.GeoJson);
                        var geometryJson = geoJson["features"]?[0]?["geometry"]?.ToString();
                        if (geometryJson != null)
                        {
                            var geometry = _geoJsonReader.Read<Geometry>(geometryJson);
                            if (geometry.Contains(point))
                                return true;
                        }
                    }
                    catch
                    {
                        // Optional: log malformed GeoJSON
                        continue;
                    }
                }
            }

            return false;
        }
    }
}
