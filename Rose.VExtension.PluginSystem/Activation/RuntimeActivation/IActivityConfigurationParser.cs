using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.PluginSystem.Runtime.RequestHandeling;

namespace Rose.VExtension.PluginSystem.Activation.RuntimeActivation
{
    public interface IActivityConfigurationParser : IConfigurationItemParser<IPluginActivity>
    {
        Plugin Plugin { get; }
    }

    public class ActivityConfigurationParser : IActivityConfigurationParser
    {
        public ActivityConfigurationParser(Plugin plugin)
        {
            Plugin = plugin;
        }

        public IPluginActivity ParseConfiguration(IConfigurationItem item)
        {
            var provider = new PluginPlatformProvider();
            var platform = provider.GetPlatform(Plugin, item);

            if(platform == null)
                throw new ConfigurationItemParseException("Не удалось создать платформу по заданной конфигурации");

            var activity = platform.CreateActivity();
            return activity;

        }

        public Plugin Plugin { get;  private set; }
    }
}
