using System;
namespace CMPNatural.Core.Extentions
{
	public static class IntExtention
	{
        public static string ConvertTimeToString(this int totalMinutes)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            string period = hours >= 12 ? "PM" : "AM";
            int twelveHourFormat = hours % 12 == 0 ? 12 : hours % 12;

            return $"{twelveHourFormat:D2}:{minutes:D2} {period}";
        }
    }
}

