using System;
using System.Linq;
using System.Reflection;
using Ninject;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Activation.RuntimeActivation
{
    /// <summary>
    /// Представляет набор методов для нахождения платформы плагина
    /// </summary>
    public class PluginPlatformProvider
    {


        public Type GetPlatformType(string platformName)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.GetInterfaces().Contains(typeof (IPluginPlatform)) &&
                        type.GetCustomAttribute<PlatformAttribute>() != null &&
                        type.GetCustomAttribute<PlatformAttribute>().Name == platformName)
                    {
                        return type;
                    }
                }

                return null;

            }
            catch 
            {
                return null;
            }
        }

        public IPluginPlatform GetPlatform(Plugin plugin, IConfigurationItem platformConfigurationItem)
        {
            var config = plugin.PluginConfiguration;
            var kernel = new StandardKernel(new InitializationModule());
            var syntax = new ConfigurationSyntax();
            var platformSection = platformConfigurationItem;
            var platformName = platformSection.GetContentValue("Type");
            var type = GetPlatformType(platformName);
            return Activator.CreateInstance(type, new object[] { plugin, platformConfigurationItem }) as IPluginPlatform;

        }

    }
}
