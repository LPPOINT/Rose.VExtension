using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rose.VExtension.PluginSystem.Runtime.RequestHandeling.FilterTemplates
{
    public class FilterTemplatesDiscoverer
    {
        public IEnumerable<Type> DiscoverAllTemplatesTypes()
        {
            var area = Assembly.GetExecutingAssembly().GetTypes();
            var types =
                area.Where(
                    type =>
                        type.GetInterfaces()
                            .Contains(typeof(IPluginRequestFilterTemplate)) &&
                                      type.GetCustomAttribute<FilterTemplateAttribute>() != null);
            return types;
        }

        public IEnumerable<IPluginRequestFilter> DiscoverAndCreateAllTemplates()
        {
            var types = DiscoverAllTemplatesTypes();
            var result = new List<IPluginRequestFilter>();

            foreach (var type in types)
            {
                try
                {
                    var filter = Activator.CreateInstance(type) as IPluginRequestFilter;
                    result.Add(filter);
                }
                catch 
                {

                }
            }

            return result;

        }

        public IPluginRequestFilterTemplate DiscoverAndCreateTemplate(string templateName)
        {
            var types = DiscoverAllTemplatesTypes();
            foreach (var type in types)
            {
                var a = type.GetCustomAttribute<FilterTemplateAttribute>();
                if (a != null)
                {
                    try
                    {
                        if (a.TemplateName == templateName)
                            return Activator.CreateInstance(type) as IPluginRequestFilterTemplate;
                    }
                    catch 
                    {
                        return null;
                    }
                }
            }

            return null;

        }
    }
}
