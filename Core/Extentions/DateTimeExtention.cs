using System;
namespace CMPNatural.Core.Extentions
{
    public static class DateTimeExtension
    {
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("h:mm tt");
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("MM/dd/yyyy");
        }
    }
}

