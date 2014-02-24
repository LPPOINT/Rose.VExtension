using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Runtime.RequestHandeling.FilterTemplates
{
    public interface IPluginRequestFilterTemplate
    {
        bool IsValidRequest(PluginRequest request, IConfigurationItem settings);
    }
}
