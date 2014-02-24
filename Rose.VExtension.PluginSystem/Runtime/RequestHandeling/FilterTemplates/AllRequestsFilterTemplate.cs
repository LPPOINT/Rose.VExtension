using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Runtime.RequestHandeling.FilterTemplates
{
    [FilterTemplate("AllRequests")]
    public class AllRequestsFilterTemplate : IPluginRequestFilterTemplate
    {
        public bool IsValidRequest(PluginRequest request, IConfigurationItem settings)
        {
            return true;
        }
    }
}
