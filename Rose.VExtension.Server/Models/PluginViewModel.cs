using Rose.VExtension.PluginSystem;

namespace Rose.VExtension.Server.Models
{
    public class PluginViewModel
    {

        public PluginViewModel()
        {
            
        }

        public PluginViewModel(Plugin plugin, DbInteraction.Plugin enity, IPluginConnection connection)
        {
            Plugin = plugin;
            Enity = enity;
            Connection = connection;
        }

        public Plugin Plugin { get; set; }
        public DbInteraction.Plugin Enity { get; set; }
        public IPluginConnection Connection { get; set; }



    }
}