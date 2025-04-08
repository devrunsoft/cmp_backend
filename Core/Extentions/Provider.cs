using System;
using GeoCoordinatePortable;
using System.Runtime.InteropServices;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMPNatural.Core.Entities
{
	public partial class Provider
    {
		public Provider()
		{
		}

        //public double Distance(double sLatitude, double sLongitude)
        //{
        //    if (Lat == null || Long == null)
        //        return double.MaxValue;

        //    var sCoord = new GeoCoordinate(sLatitude, sLongitude);
        //    var eCoord = new GeoCoordinate((double)Lat, (double)Long);
        //    var dis = sCoord.GetDistanceTo(eCoord);
        //    return dis;
        //}

        private bool IsDistanceIn(double sLatitude, double sLongitude,ServiceArea model)
        {
            if (Lat == null || Long == null)
                return false;

            var sCoord = new GeoCoordinate(sLatitude, sLongitude);
            var eCoord = new GeoCoordinate(model.Lat.Value, model.Lng.Value);
            var dis = sCoord.GetDistanceTo(eCoord);
            return dis <= model.Radius;
        }
        public bool IsPointInCity(double lat, double lng)
        {
            var point = new Point(lng, lat);
            var reader = new GeoJsonReader();

            foreach (var item in ServiceArea)
            {
                if (item.ServiceAreaType == Enums.ServiceAreaTypeEnum.Circle)
                {
                     if(IsDistanceIn(lat, lng, item))
                    {
                        return true;
                    }
                }
                else
                {
                    var jObject = JObject.Parse(item.GeoJson);
                    var geometryJson = jObject["features"]?[0]?["geometry"]?.ToString();
                    if (geometryJson != null)
                    {
                        var geometry = reader.Read<Geometry>(geometryJson);
                        if (geometry.Contains(point))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}


