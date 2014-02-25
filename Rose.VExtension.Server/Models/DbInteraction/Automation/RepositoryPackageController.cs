namespace Rose.VExtension.Server.Models.DbInteraction.Automation
{
    public class RepositoryPackageController : RepositoryPluginComponentEntityController<PluginPackage, int>
    {
        public RepositoryPackageController(IPluginsRepository repository, PluginsContainer dbContext) : base(repository, dbContext, "Id", "PluginId")
        {
        }

        public override void AddEntity(PluginPackage entity)
        {
            var plugin = Repository.PluginContext.GetEntity(entity.PluginId);

            if (plugin != null)
                plugin.PluginPackage = entity;


            DbContext.PluginPackages.Add(entity);
            DbContext.SaveChanges();
        }
    }
}