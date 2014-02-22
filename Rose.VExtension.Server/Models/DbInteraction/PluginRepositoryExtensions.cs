using System;

namespace Rose.VExtension.Server.Models.DbInteraction
{
    public static class PluginRepositoryExtensions
    {
        public static PluginLocation GetPluginStatus(this IPluginsRepository repository, string pluginId)
        {
            var plugin = repository.GetPlugin(pluginId);

            if (plugin != null)
            {
                return plugin.Location;
            }

            return PluginLocation.Undefined;
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