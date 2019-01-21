using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BleemSync.UI
{
    [HtmlTargetElement("button")]
    public class ButtonHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpRequest _request;
        private IDictionary<string, string> _routeValues;
        private IUrlHelperFactory _urlHelperFactory;
        private IUrlHelper _urlHelper;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                    _routeValues = (IDictionary<string, string>)new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ButtonHelper(IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory)
        {
            _generator = generator;
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            var isRaised = output.Attributes.SingleOrDefault(a => a.Name == "raised") != null;
            var isFlat = output.Attributes.SingleOrDefault(a => a.Name == "flat") != null;
            var isOutline = output.Attributes.SingleOrDefault(a => a.Name == "outline") != null;
            var isFloating = output.Attributes.SingleOrDefault(a => a.Name == "floating") != null;
            var isLg = output.Attributes.SingleOrDefault(a => a.Name == "lg") != null;
            var isSm = output.Attributes.SingleOrDefault(a => a.Name == "sm") != null;
            var isBlock = output.Attributes.SingleOrDefault(a => a.Name == "block") != null;
            // Colors
            var isDefault = output.Attributes.SingleOrDefault(a => a.Name == "default") != null;
            var isPrimary = output.Attributes.SingleOrDefault(a => a.Name == "primary") != null;
            var isSuccess = output.Attributes.SingleOrDefault(a => a.Name == "success") != null;
            var isInfo = output.Attributes.SingleOrDefault(a => a.Name == "info") != null;
            var isWarning = output.Attributes.SingleOrDefault(a => a.Name == "warning") != null;
            var isDanger = output.Attributes.SingleOrDefault(a => a.Name == "danger") != null;
            var isLink = output.Attributes.SingleOrDefault(a => a.Name == "link") != null;

            var icon = output.Attributes.SingleOrDefault(a => a.Name == "icon");
            var text = output.Attributes.SingleOrDefault(a => a.Name == "text");
            var action = output.Attributes.SingleOrDefault(a => a.Name == "asp-action");
            var controller = output.Attributes.SingleOrDefault(a => a.Name == "asp-controller");
            var buttonClassAttr = output.Attributes.SingleOrDefault(a => a.Name == "class");

            output.Attributes.RemoveAll("raised");
            output.Attributes.RemoveAll("flat");
            output.Attributes.RemoveAll("outline");
            output.Attributes.RemoveAll("floating");
            output.Attributes.RemoveAll("lg");
            output.Attributes.RemoveAll("sm");
            output.Attributes.RemoveAll("block");
            output.Attributes.RemoveAll("default");
            output.Attributes.RemoveAll("primary");
            output.Attributes.RemoveAll("success");
            output.Attributes.RemoveAll("info");
            output.Attributes.RemoveAll("warning");
            output.Attributes.RemoveAll("danger");
            output.Attributes.RemoveAll("link");
            output.Attributes.RemoveAll("icon");
            output.Attributes.RemoveAll("text");

            var buttonClass = "btn";

            if (buttonClassAttr != null)
            {
                buttonClass += " " + buttonClassAttr.Value;
            }

            var labelAttributes = new Dictionary<string, object>();

            if (isRaised) buttonClass = $"{buttonClass} pmd-btn-raised";
            if (isFlat) buttonClass = $"{buttonClass} pmd-btn-flat";
            if (isOutline) buttonClass = $"{buttonClass} pmd-btn-outline";
            if (isFloating) buttonClass = $"{buttonClass} pmd-btn-fab";
            if (isLg) buttonClass = $"{buttonClass} btn-lg";
            if (isSm) buttonClass = $"{buttonClass} btn-sm";
            if (isBlock) buttonClass = $"{buttonClass} btn-block";
            if (isDefault) buttonClass = $"{buttonClass} btn-default";
            if (isPrimary) buttonClass = $"{buttonClass} btn-primary";
            if (isSuccess) buttonClass = $"{buttonClass} btn-success";
            if (isInfo) buttonClass = $"{buttonClass} btn-info";
            if (isWarning) buttonClass = $"{buttonClass} btn-warning";
            if (isDanger) buttonClass = $"{buttonClass} btn-danger";
            if (isLink) buttonClass = $"{buttonClass} btn-link";

            if (!isDefault && !isPrimary && !isSuccess && !isInfo && !isWarning && !isDanger && !isLink)
            {
                buttonClass = $"{buttonClass} btn-primary";
            }

            var inputPre = "";
            var inputPost = "";
            var labelClass = "control-label";

            if (icon != null)
            {
                inputPre = $"{inputPre}<div class=\"input-group\"><div class=\"input-group-addon\"><i class=\"material-icons md-dark pmd-sm\">{icon.Value}</i></div>";
                inputPost = $"{inputPost}</div>";
                labelClass = $"{labelClass} pmd-input-group-label";
            }

            labelAttributes.Add("class", labelClass);


            string buttonOutput = "";

            if (Action != null || Controller != null)
            {
                var url = _urlHelper.Action(Action, Controller, _routeValues);

                output.Attributes.SetAttribute("href", url);
                output.TagName = "a";
            }
            else
            {
                
                output.Attributes.SetAttribute("type", "submit");
                output.TagName = "button";
            }

            buttonOutput = text.Value.ToString();

            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", buttonClass);
            output.Content.SetHtmlContent(buttonOutput);
        }
    }
}
