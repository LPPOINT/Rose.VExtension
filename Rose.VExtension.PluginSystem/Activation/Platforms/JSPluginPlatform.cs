using System.Collections.Generic;
using Ninject;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Activation.Platforms
{

    /// <summary>
    /// Представляет платформу на языке Javascript
    /// </summary>
    [Platform("JS")]
    public class JSPluginPlatform : IPluginPlatform
    {

        public JSPluginPlatform(Plugin plugin)
        {
            Plugin = plugin;
            Type = PlatformType.Javascript;

            Scripts = new List<string>();

            try
            {
                var config = plugin.PluginConfiguration;
                var kernel = new StandardKernel(new InitializationModule());
                var syntax = kernel.Get<IConfigurationSyntax>();

                var scripts = config.GetItem(syntax.ScriptsItem);

                foreach (var script in scripts.Content)
                {
                    Scripts.Add(script.Value);
                }

                EntryFunction = config.GetItemValue(syntax.JSEntryFunction);

            }
            catch 
            {
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

        public void BuildDomain()
        {
            var domain = new JavascriptDomain();

            domain.Plugin = Plugin;
            Plugin.Domain = domain;
        }
    }
}
