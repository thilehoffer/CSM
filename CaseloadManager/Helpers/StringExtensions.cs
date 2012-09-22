using System.Globalization;
using System.Text;

namespace CaseloadManager.Helpers
{
    public static class StringExtensions
    {
        public static string Left(this string input, int length)
        {
            var result = input;
            if (input != null && input.Length > length)
            {
                result = input.Substring(0, length);
            }
            return result;
        }

        public static string Right(this string input, int length)
        {
            string result = input;
            if (input != null && input.Length > length)
            {
                result = input.Substring(input.Length - length, length);
            }
            return result;
        }

        public static string FormatAsPhone(this string input)
        {
            
            if (string.IsNullOrEmpty(input) || input.Length != 10)
                return input;

            return input.Left(3) + "-" + input.Substring(3, 3) + "-" + input.Right(4);
        }

        public static string RemoveNonNumericCharacters(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var sb = new StringBuilder();
            foreach (var c in input.ToCharArray())
            {
                var s = c.ToString(CultureInfo.InvariantCulture);
                var d = new double();
                if (double.TryParse(s, out d))
                    sb.Append(s);
            }
            return sb.ToString();
        }
    }
}