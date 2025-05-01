using System;
namespace CMPNatural.Core.Extentions
{
	public static class PhoneNumberExtention
	{
      public static string FormatPhoneNumber(this string phoneNumber)
        {
            // Remove any non-digit characters (just in case)
            string digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Check if it starts with '1' and is 11 digits long (U.S. country code)
            if (digits.Length == 11 && digits.StartsWith("1"))
            {
                // Remove the leading '1'
                digits = digits.Substring(1);
            }

            // Now it should be exactly 10 digits
            if (digits.Length == 10)
            {
                return $"({digits.Substring(0, 3)}) {digits.Substring(3, 3)}-{digits.Substring(6, 4)}";
            }
            else
            {
                // If not valid, return the original input or handle it as you want
                return phoneNumber;
            }
        }

	}
}

