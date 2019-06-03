using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVC5RealWorld.Util
{
    [HtmlTargetElement("input", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class InvariantDecimalTagHelper : InputTagHelper
    {
        private const string ForAttributeName = "asp-for-invariant";

        private IHtmlGenerator _generator;
        
        [HtmlAttributeName(ForAttributeName)]
        public bool IsInvariant { set; get; }

        public InvariantDecimalTagHelper(IHtmlGenerator generator) : base(generator)
        {
                       

            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
           
            if (IsInvariant && output.TagName == "input" && For.Model != null && For.Model.GetType() == typeof(decimal))
            {
                decimal value = (decimal)(For.Model);
                var invariantValue = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                output.Attributes.SetAttribute(new TagHelperAttribute("value", invariantValue));
            }

            base.Process(context, output);
        }
    }
}
