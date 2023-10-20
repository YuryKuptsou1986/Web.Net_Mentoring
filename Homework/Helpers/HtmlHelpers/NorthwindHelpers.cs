using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Homework.Helpers.HtmlHelpers
{
    public static class NorthwindHelpers
    {
        public static IHtmlContent NorthwindImageLink(this IHtmlHelper htmlHelper, int imageId, string linkText)
            => new HtmlString($"<a href=\"images/{imageId}\">{linkText}</a>");
    }
}
