using System.Linq;

namespace Rose.VExtension.Server.Models.DbInteraction.Automation
{
    public class RepositoryPluginController : RepositoryEntityController<Plugin, string>
    {
        public RepositoryPluginController(IPluginsRepository repository, PluginsContainer dbContext, string idProperty) : base(repository, dbContext, idProperty)
        {
        }

        public void SetPluginLocation(string pluginId, PluginLocation location)
        {

            var plugin = (from p in Set.Local where p.Id == pluginId select p).FirstOrDefault();
            if (plugin != null)
            {
                plugin.Location = location;
                DbContext.SaveChanges();
            }
        }

    }
}