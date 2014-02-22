using System;

namespace Rose.VExtension.PluginSystem.Activation.Platforms
{
    public class PlatformAttribute : Attribute
    {
        public PlatformAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
