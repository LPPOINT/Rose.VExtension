using System;
using System.Runtime.Serialization;

namespace Rose.VExtension.PluginSystem.Configuration
{

    public class WrapperException : Exception
    {
        public WrapperException()
        {
        }

        public WrapperException(string message) : base(message)
        {
        }

        public WrapperException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrapperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class UnexpectedConfigItemForWrappingException : Exception
    {
        public UnexpectedConfigItemForWrappingException()
        {
        }

        public UnexpectedConfigItemForWrappingException(string message) : base(message)
        {
        }

        public UnexpectedConfigItemForWrappingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public interface IConfigurationItemWrapper
    {
        void Wrap(IConfigurationItem item);
        IConfigurationItem UnWrap();
    }
}
