using System;

namespace Rose.VExtension.Server.Models
{
    public static class PluginRepositoryExtensions
    {
        public static Models.PluginLocation GetPluginStatus(this IPluginsRepository repository, string pluginId)
        {
            var plugin = repository.GetPlugin(pluginId);

            if (plugin != null)
            {
                return plugin.Location;
            }

            return Models.PluginLocation.Undefined;
        }
        public static string GetPluginName(this IPluginsRepository repository, string pluginId)
        {
            var plugin = repository.GetPlugin(pluginId);

            if (plugin != null)
            {
                return plugin.Name;
            }

            return string.Empty;
        }
        public static Version GetPluginVersion(this IPluginsRepository repository, string pluginId)
        {
            var plugin = repository.GetPlugin(pluginId);

            if (plugin != null)
            {
                return Version.Parse(plugin.Version);
            }

            return null;
        }
    }
}