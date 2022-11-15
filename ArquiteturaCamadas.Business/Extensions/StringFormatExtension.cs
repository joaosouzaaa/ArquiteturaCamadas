using System.Text.RegularExpressions;

namespace ArquiteturaCamadas.Business.Extensions
{
    public static class StringFormatExtension
    {
        public static string FormatTo(this string message, params object[] args) =>
            string.Format(message, args);

        public static string CleanCaracters(this string value) => Regex.Replace(value, @"[^\d]", "");
    }
}
