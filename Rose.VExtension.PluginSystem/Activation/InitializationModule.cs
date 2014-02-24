using Ninject.Modules;
using Rose.VExtension.PluginSystem.Activation.RuntimeActivation;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Validation;

namespace Rose.VExtension.PluginSystem.Activation
{
    public class InitializationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPluginValidator>().To<PluginValidator>();
            Bind<IConfigurationSyntax>().To<ConfigurationSyntax>();
            Bind<IPluginControllerInitializer>().To<PluginControllerStepProvider>();
        }
    }
}
