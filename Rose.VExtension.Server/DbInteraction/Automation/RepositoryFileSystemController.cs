using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.DbInteraction.Automation
{
    public class RepositoryFileSystemController : RepositoryPluginComponentEntityController<PluginFileSystem, int>
    {
        public RepositoryFileSystemController(IPluginsRepository repository, PluginsContainer dbContext) : base(repository, dbContext, "Id", "PluginId")
        {
        }

        public override void AddEntity(PluginFileSystem entity)
        {
            var plugin = Repository.PluginContext.GetEntity(entity.PluginId);

            if (plugin != null)
                plugin.PluginFileSystem = entity;

            DbContext.PluginFileSystems.Add(entity);
            DbContext.SaveChanges();
        }
    }
}