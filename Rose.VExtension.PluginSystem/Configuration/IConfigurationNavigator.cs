using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Configuration
{
    public interface IConfigurationNavigator
    {
        IConfigurationItem GetItem(IPluginConfiguration pluginConfiguration);
    }

    public abstract class ConfigurationNavigator<TWrapperType> : IConfigurationNavigator where TWrapperType : IConfigurationItemWrapper, new()
    {
        public abstract IConfigurationItem GetItem(IPluginConfiguration pluginConfiguration);

        public TWrapperType GetItemWrapper(IPluginConfiguration pluginConfiguration)
        {
            var item = GetItem(pluginConfiguration);
            if (item == null)
                return default(TWrapperType);
            return item.WrapTo<TWrapperType>();
        }
    }

}
