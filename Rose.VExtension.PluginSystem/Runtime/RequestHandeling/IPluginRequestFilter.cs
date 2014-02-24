using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Runtime.RequestHandeling.FilterTemplates;

namespace Rose.VExtension.PluginSystem.Runtime.RequestHandeling
{
    public interface IPluginRequestFilter
    {
        bool IsValidRequest(PluginRequest request);
    }

    public class PluginRequestFilterBase : IPluginRequestFilter
    {
        public virtual bool IsValidRequest(PluginRequest request)
        {
            return true;
        }
    }

    public class PluginRequestTemplateBasedFilter : PluginRequestFilterBase
    {
        public PluginRequestTemplateBasedFilter(IPluginRequestFilterTemplate template, IConfigurationItem settings)
        {
            Settings = settings;
            Template = template;
        }

        public IPluginRequestFilterTemplate Template { get; private set; }
        public IConfigurationItem Settings { get; private set; }

        public override bool IsValidRequest(PluginRequest request)
        {
            return Template.IsValidRequest(request, Settings);
        }
    }

}
