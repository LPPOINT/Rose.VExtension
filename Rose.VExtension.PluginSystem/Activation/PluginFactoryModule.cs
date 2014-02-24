using Ninject.Modules;
using Rose.VExtension.PluginSystem.Activation.RuntimeActivation;
using Rose.VExtension.PluginSystem.Packing;

namespace Rose.VExtension.PluginSystem.Activation
{
    public class PluginFactoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPluginFactory>().To<PluginFactory>();
            Bind<IPluginPackageService>().To<ZipPluginPackageService>();
            Bind<IPluginPackageFileSystem>().To<ZipPluginPackageFileSystem>();
            Bind<IPluginControllerInitializer>().To<PluginControllerStepProvider>();
        }
    }
}
