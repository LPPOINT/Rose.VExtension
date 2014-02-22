using System;
using System.Runtime.Serialization;

namespace Rose.VExtension.PluginSystem.Packing
{
    public class ExtractionException : Exception
    {
        public ExtractionException()
        {
        }

        public ExtractionException(string message) : base(message)
        {
        }

        public ExtractionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExtractionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
