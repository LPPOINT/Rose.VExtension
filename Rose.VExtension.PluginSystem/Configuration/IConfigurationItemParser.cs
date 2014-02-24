using System;
using System.Runtime.Serialization;

namespace Rose.VExtension.PluginSystem.Configuration
{

    public class ConfigurationItemParseException : Exception
    {
        public ConfigurationItemParseException()
        {
        }

        public ConfigurationItemParseException(string message) : base(message)
        {
        }

        public ConfigurationItemParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigurationItemParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public interface IConfigurationItemParser<out T>
    {
        T ParseConfiguration(IConfigurationItem item);
    }
}
