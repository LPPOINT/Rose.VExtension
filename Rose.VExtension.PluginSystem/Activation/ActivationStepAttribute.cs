using System;

namespace Rose.VExtension.PluginSystem.Activation
{
    public class ActivationStepAttribute : Attribute
    {
        public ActivationStepAttribute(ActivationStepName name)
        {
            Name = name;
        }

        public ActivationStepName Name { get; private set; }
    }
}
