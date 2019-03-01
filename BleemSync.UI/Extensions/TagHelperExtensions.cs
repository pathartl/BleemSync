using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.UI
{
    public static class TagHelperExtensions
    {
        public static void AddClass(this TagHelperOutput output, string @class)
        {
            var classAttribute = output.Attributes.Where(a => a.Name == "class").FirstOrDefault();

            if (classAttribute == null)
            {
                output.Attributes.SetAttribute("class", @class);
            }
            else
            {
                var currentClass = classAttribute.Value;

                output.Attributes.SetAttribute("class", $"{currentClass} {@class}");
            }
        }

        public static bool HasAttribute(this TagHelperOutput output, string attribute)
        {
            return output.Attributes.SingleOrDefault(a => a.Name == attribute) != null;
        }

        public static string GetAttribute(this TagHelperOutput output, string attribute)
        {
            return output.Attributes.SingleOrDefault(a => a.Name == attribute).Value.ToString();
        }

        public static void SetAttribute(this TagHelperOutput output, string attribute, string value)
        {
            output.Attributes.SetAttribute(attribute, value);
        }

        public static void RemoveAttribute(this TagHelperOutput output, string attribute)
        {
            output.Attributes.RemoveAll(attribute);
        }

        public static void RemoveAttributes(this TagHelperOutput output, IEnumerable<string> attributes)
        {
            foreach (var attribute in attributes)
            {
                RemoveAttribute(output, attribute);
            }
        }
    }
}
