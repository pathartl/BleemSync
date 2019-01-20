namespace BleemSync.UI
{
    public class TagHelperAttribute : Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute
    {
        public TagHelperAttribute(string name) : base(name) { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
