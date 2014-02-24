using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Activation.RuntimeActivation
{

    /// <summary>
    /// Представляет платформу на языке Javascript
    /// </summary>
    [Platform("JS")]
    public class JSPluginPlatform : IPluginPlatform
    {

        public JSPluginPlatform(Plugin plugin, IConfigurationItem platformActivityItem)
        {
            Plugin = plugin;
            Type = PlatformType.Javascript;
            ConfigurationItem = platformActivityItem;
            Scripts = new List<string>();

            try
            {
                var config = plugin.PluginConfiguration;
                var kernel = new StandardKernel(new InitializationModule());
                var syntax = kernel.Get<IConfigurationSyntax>();


                foreach (var script in platformActivityItem.Content.Where(pair => pair.Key == "Script"))
                {
                    Scripts.Add(script.Value);
                }

                EntryFunction = platformActivityItem.GetContentValue("EntryFunction");

            }
            catch (Exception e)
            {
                throw new Exception("Возникла ошибка при создании платформы", e);
            }

        }

        public Plugin Plugin { get; private set; }

        /// <summary>
        /// Список имен файлов скриптов, которые будут использованы при запуске плагина
        /// </summary>
        public List<string> Scripts { get; private set; }

        /// <summary>
        /// Имя функции в javascript-коде, которая будет выполнена при запуске контроллера плагина
        /// </summary>
        public string EntryFunction { get; private set; }
        public PlatformType Type { get; private set; }
        public IConfigurationItem ConfigurationItem { get; private set; }

        public IPluginActivity CreateActivity()
        {
            var activity = new JavascriptActivity();
            activity.Platform = this;
            activity.Name = this.GetActivityName();
            activity.Plugin = Plugin;
            return activity;
        }
    }
}
