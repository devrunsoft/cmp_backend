using System;
using GeoCoordinatePortable;
using System.Runtime.InteropServices;

namespace CMPNatural.Core.Entities
{
	public partial class Provider
    {
		public Provider()
		{
		}

        public double Distance(double sLatitude, double sLongitude)
        {
            if (Lat == null || Long == null)
                return double.MaxValue;

            var sCoord = new GeoCoordinate(sLatitude, sLongitude);
            var eCoord = new GeoCoordinate((double)Lat, (double)Long);

            return sCoord.GetDistanceTo(eCoord);
        }
    }
}


