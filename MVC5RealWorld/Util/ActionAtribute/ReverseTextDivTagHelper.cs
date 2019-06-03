using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Util.ActionAtribute
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("div-reverso")] // Not using this.
    public class ReverseTextDivTagHelper : TagHelper
    {
        private string _divData = string.Empty;
        public string DivData
        {
            get
            {
                char[] reversedData = _divData.ToCharArray();
                Array.Reverse(reversedData);
                String sDataReversed = new String(reversedData);
                return AllCaps ? sDataReversed.ToUpper() : sDataReversed;
            }
            set { _divData = value; }
        }

        public bool AllCaps { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Content.SetContent(DivData);
        }
    }
}
