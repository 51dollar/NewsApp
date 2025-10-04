using System.Text.RegularExpressions;

namespace WebNews.Helpers;

public static class HtmlTagHelper
{
    public static string RemoveHtmlTags(string input)
    {
        return Regex.Replace(input, "<.*?>|&.*?;", string.Empty);
    }
}