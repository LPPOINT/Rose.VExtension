using System;
using System.Linq;
using System.Reflection;
using Ninject;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Activation.Platforms
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

        public IPluginPlatform GetPlatform(Plugin plugin)
        {
            var config = plugin.PluginConfiguration;
            var kernel = new StandardKernel(new InitializationModule());
            var syntax = kernel.Get<IConfigurationSyntax>();
            var platformSection = config.GetItem(syntax.PlatformItem);
            var platformName = platformSection.GetContentValue("Name");
            var type = GetPlatformType(platformName);
            return Activator.CreateInstance(type, plugin) as IPluginPlatform;

        }

    }
}
