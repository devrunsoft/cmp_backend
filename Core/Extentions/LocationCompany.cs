using System;
using GeoCoordinatePortable;

namespace CMPNatural.Core.Entities
{
	public  partial class LocationCompany
	{
      public static  double MetersPerMile = 1609.344;

       public static double DistanceMeters(double lat1, double lng1, double lat2, double lng2)
        {
            var a = new GeoCoordinate(lat1, lng1);
            var b = new GeoCoordinate(lat2, lng2);
            return a.GetDistanceTo(b);
        }

      public static List<List<T>> ClusterByRadius<T>(
            IReadOnlyList<T> items,
            Func<T, double?> latSel,
            Func<T, double?> lngSel,
            double radiusMeters)
        {
            var clusters = new List<List<T>>();
            var used = new bool[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                if (used[i]) continue;
                var latI = latSel(items[i]); var lngI = lngSel(items[i]);
                if (latI == null || lngI == null) { used[i] = true; continue; }

                var cluster = new List<T> { items[i] };
                used[i] = true;

                for (int j = i + 1; j < items.Count; j++)
                {
                    if (used[j]) continue;
                    var latJ = latSel(items[j]); var lngJ = lngSel(items[j]);
                    if (latJ == null || lngJ == null) { used[j] = true; continue; }

                    if (DistanceMeters(latI.Value, lngI.Value, latJ.Value, lngJ.Value) <= radiusMeters)
                    {
                        cluster.Add(items[j]);
                        used[j] = true;
                    }
                }
                clusters.Add(cluster);
            }
            return clusters;
        }
    }
}

