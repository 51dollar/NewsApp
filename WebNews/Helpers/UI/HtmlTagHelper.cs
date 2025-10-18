using System.Text.RegularExpressions;

namespace WebNews.Helpers.UI;

public static class HtmlTagHelper
{
    public static string RemoveHtmlTags(string input)
    {
        return Regex.Replace(input, "<.*?>|&.*?;", string.Empty);
    }
}