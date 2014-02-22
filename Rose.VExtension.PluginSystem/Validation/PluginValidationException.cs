using System;

namespace Rose.VExtension.PluginSystem.Validation
{
    public class PluginValidationException : Exception
    {
        public PluginValidationException()
        {
        }

        public PluginValidationException(string message) : base(message)
        {
        }

        public PluginValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
