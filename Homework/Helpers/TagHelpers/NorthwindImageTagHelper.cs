using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Homework.Helpers.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "northwind-id")]
    public class NorthwindImageTagHelper : TagHelper
    {
        public string NorthwindId { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("northwind-id");
            output.Attributes.SetAttribute("href", $"images/{NorthwindId}");
            return base.ProcessAsync(context, output);
        }
    }
}
