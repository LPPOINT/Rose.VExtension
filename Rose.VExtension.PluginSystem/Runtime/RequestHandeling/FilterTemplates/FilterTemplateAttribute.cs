using System;

namespace Rose.VExtension.PluginSystem.Runtime.RequestHandeling.FilterTemplates
{
    public class FilterTemplateAttribute : Attribute
    {
        public FilterTemplateAttribute(string templateName)
        {
            TemplateName = templateName;
        }

        public string TemplateName { get; private set; }
    }
}
