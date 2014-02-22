using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Activation.Platforms
{

    public class PluginControllerInitializationException : Exception
    {
        public PluginControllerInitializationException()
        {
        }

        public PluginControllerInitializationException(string message)
            : base(message)
        {
        }

        public PluginControllerInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class PluginControllerStepProvider : IPluginControllerInitializer
    {

        [ActivationStep(ActivationStepName.ControllerActivation)]
        public void InitializeController(Plugin plugin, ActivationInfo info)
        {
            var platformProvider = new PluginPlatformProvider();
            var platform = platformProvider.GetPlatform(plugin);


            if (platform == null)
                throw new PluginControllerInitializationException("Не удалось определить платформу плагина");


            plugin.Platform = platform;
            platform.BuildDomain();

        }
    }
}
