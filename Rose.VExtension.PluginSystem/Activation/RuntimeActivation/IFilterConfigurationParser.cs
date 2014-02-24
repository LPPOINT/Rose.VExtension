using System.Linq;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Runtime.RequestHandeling;
using Rose.VExtension.PluginSystem.Runtime.RequestHandeling.FilterTemplates;

namespace Rose.VExtension.PluginSystem.Activation.RuntimeActivation
{
    public interface IFilterConfigurationParser : IConfigurationItemParser<IPluginRequestFilter>
    {
    }

    public class FilterConfigurationParser : IFilterConfigurationParser
    {
        public IPluginRequestFilter ParseConfiguration(IConfigurationItem item)
        {

            if (item.Content.ContainsKey("template"))
            {
                var name = item.GetContentValue("template");
                var discoverer = new FilterTemplatesDiscoverer();
                var template = discoverer.DiscoverAndCreateTemplate(name);

                if(template == null)
                    throw new ConfigurationItemParseException(string.Format("Не найден шаблон фильтра с именем '{0}'", name));

                return new PluginRequestTemplateBasedFilter(template, item);

            }

            return new PluginRequestFilterBase();
        }
    }

}
