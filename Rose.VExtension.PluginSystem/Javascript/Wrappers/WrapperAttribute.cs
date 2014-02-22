using System;

namespace Rose.VExtension.PluginSystem.Javascript.Wrappers
{
    public class WrapperAttribute : Attribute
    {
        public WrapperAttribute(Type sourceType)
        {
            SourceType = sourceType;
        }

        public Type SourceType { get; private set; }
    }
}
