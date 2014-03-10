using System.Linq;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.DbInteraction.Automation
{
    public class RepositoryStorageController : RepositoryPluginComponentEntityController<StorageItem, int>
    {
        public RepositoryStorageController(IPluginsRepository repository, PluginsContainer dbContext, string idProperty, string pluginIdProperty) : base(repository, dbContext, idProperty, pluginIdProperty)
        {
            
        }

        public StorageItem GetItemByName(string pluginId, string name)
        {
            return Set.Local.FirstOrDefault(item => item.Name == name && item.PluginId == pluginId);
        }

        public void RemoveItemByName(string pluginId, string name)
        {
            var item = GetItemByName(pluginId, name);
            RemoveEntity(item.Id);
        }

        public void AddEntity(string name, string value, string pluginId)
        {
            var entity = new StorageItem()
                         {
                             Name = name,
                             Value = value,
                             PluginId = pluginId
                         };
            AddEntity(entity);
        }

        public void SetItemValue(string pluginId, string name, string value)
        {
            var all = GetEntitiesByPluginId(pluginId);
            foreach (var storageItem in all)
            {
                if (storageItem.Name == name)
                {
                    storageItem.Value = value;
                    
                }
            }
            DbContext.SaveChanges();
        }

    }
}