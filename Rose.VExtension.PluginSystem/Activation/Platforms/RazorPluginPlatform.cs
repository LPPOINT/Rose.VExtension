using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninject;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Activation.Platforms
{
    [Platform("Razor")]
    public class RazorPluginPlatform : IPluginPlatform
    {
        public RazorPluginPlatform(Plugin plugin)
        {
            Plugin = plugin;
            Pages = new Dictionary<IPluginRequestFilter, string>();
            Type = PlatformType.Razor;
            var config = plugin.PluginConfiguration;
            var kernel = new StandardKernel(new InitializationModule());
            var syntax = kernel.Get<IConfigurationSyntax>();
            var pagesFiltersRoot = config.GetItem(syntax.RazorPagesFilters);
            var pageFilters = pagesFiltersRoot.InnerItems.Where(item => item.Name.ToLower() == "filter");

            foreach (var configurationItem in pageFilters)
            {
                var pageFile = configurationItem.GetContentValue("Page");
                var filterAction = configurationItem.GetContentValue("Action");

                using (var fileStream = Plugin.FileSystem.GetItemStream(FileSystemItem.GetRazorPageItem(pageFile)))
                {
                    var filter = new PluginRequestFilter(filterAction);

                    using (var reader = new StreamReader(fileStream))
                    {
                        var pageText = reader.ReadToEnd();
                        Pages.Add(filter, pageText);
                    }
                }

            }

        }

        public Plugin Plugin { get; private set; }

        public PlatformType Type { get; private set; }

        public Dictionary<IPluginRequestFilter, string> Pages; 

        public void BuildDomain()
        {
            var domain = new RazorDomain();

            domain.Plugin = Plugin;
            Plugin.Domain = domain;
        }
    }
}
