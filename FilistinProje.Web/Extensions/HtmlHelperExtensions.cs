using FilistinProje.Web.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FilistinProje.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString SanitizeHtml(this IHtmlHelper htmlHelper, string? html)
        {
            return new HtmlString(HtmlSanitizerHelper.Sanitize(html));
        }
    }
}