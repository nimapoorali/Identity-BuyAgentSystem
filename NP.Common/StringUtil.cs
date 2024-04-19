using PluralizeService.Core;
using System.Text;

namespace NP.Common
{
    public static class StringUtil
    {
        private static readonly Random random = new();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string Fix(this string text)
        {
            if (text is null)
            {
                return null;
            }

            text =
                text.Trim();

            if (text == string.Empty)
            {
                return null;
            }

            while (text.Contains("  "))
            {
                text =
                    text.Replace("  ", " ");
            }

            return text;
        }

        public static string RandomAlphaNumeric(int length)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomAlphaNumeric(int minLength, int maxLenth)
        {
            return new string(Enumerable.Repeat(chars, random.Next(minLength, maxLenth))
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Pluralize(this string word)
        {
            return PluralizationProvider.Pluralize(word);
        }

        public static string RandomNumeric(int length)
        {
            if (length > 0)
            {
                StringBuilder sbMinValue = new StringBuilder("1", length);
                StringBuilder sbMaxValue = new StringBuilder("9", length);

                for (int i = 0; i < length - 1; i++)
                {
                    sbMinValue.Append("0");
                    sbMaxValue.Append("9");
                }
                var minValue = int.Parse(sbMinValue.ToString());
                var maxValue = int.Parse(sbMaxValue.ToString());

                return random.Next(minValue, maxValue).ToString();
            }

            return string.Empty;
        }
    }
}