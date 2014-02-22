using Ninject.Modules;

namespace Rose.VExtension.PluginSystem.Resources
{
    public class PluginDataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPluginResourceTokenGenerator>().To<PluginResourcesTokenGenerator>();
        }
    }
}
