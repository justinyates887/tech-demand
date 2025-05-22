using System.Net;
using System.Text.RegularExpressions;

namespace TechStax.Services
{
    public static class HtmlUtils
    {
        // remove tags
        private static readonly Regex TagRegex = new Regex("<.*?>", RegexOptions.Compiled);
        public static string StripHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // strip tags, decode entities, collapse whitespace
            var withoutTags = TagRegex.Replace(input, " ");
            var decoded = WebUtility.HtmlDecode(withoutTags);
            return Regex.Replace(decoded, @"\s+", " ").Trim();
        }
    }
}
