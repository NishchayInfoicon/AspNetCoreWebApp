using System.Text.Json.Serialization;

namespace Practice.Foundation.Infrastructure.Types
{
    public class TemplateItem : ContentItem
    {
        public TemplateItem()
        {

        }

        public TemplateItem(string className) : base(className)
        {

        }
    }
}
