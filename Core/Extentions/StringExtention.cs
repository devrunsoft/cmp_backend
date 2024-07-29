namespace Barbara.Core.Extentions
{
    static public class StringExtention
    {
        public static string toLotinNumber(this string input)
        {
            string[] arabicNumbers = { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
            string[] persian = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString()).Replace(arabicNumbers[j], j.ToString());

            return input;
        }
    }
}
