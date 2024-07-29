using System;
using System.Globalization;
namespace Barbara.Application.Services
{
    public static class DateTimeConvertor
    {
        public static string ToReservTimeAndDate(this DateTime now)
        {
      

            PersianCalendar persianCalendar = new PersianCalendar();
            int year = persianCalendar.GetYear(now);
            int month = persianCalendar.GetMonth(now);
            int day = persianCalendar.GetDayOfMonth(now);
            int hour = persianCalendar.GetHour(now);
            int minute = persianCalendar.GetMinute(now);
            //int second = persianCalendar.GetSecond(now);

            string persianDate = $"{year}/{month.ToString("D2")}/{day.ToString("D2")}";
            string persianTime = $"{hour.ToString("D2")}:{minute.ToString("D2")}";
            return $"{persianTime} {persianDate}";
        }
    }
}

