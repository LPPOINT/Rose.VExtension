using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Common;

namespace Rose.VExtension.PluginSystem.Helpers.VKElements
{
    [Serializable]
    public class VKPageElementException : Exception
    {
        public VKPageElementException()
        {
        }

        public VKPageElementException(string message) : base(message)
        {
        }

        public VKPageElementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VKPageElementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class VKPageElementReadException : VKPageElementException
    {


        public VKPageElementReadException()
        {
        }

        public VKPageElementReadException(string message) : base(message)
        {
        }

        public VKPageElementReadException(string message, Exception inner) : base(message, inner)
        {
        }

        protected VKPageElementReadException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class VKPageElementWriteException : VKPageElementException
    {

        public VKPageElementWriteException()
        {
        }

        public VKPageElementWriteException(string message) : base(message)
        {
        }

        public VKPageElementWriteException(string message, Exception inner) : base(message, inner)
        {
        }

        protected VKPageElementWriteException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    public interface IVKPageElement
    {
        HtmlNode Node { get; }
    }

    public class VKPageElement : IVKPageElement
    {
        public VKPageElement(HtmlNode node)
        {
            Check.NotNull(node);
            Node = node;
        }

        public HtmlNode Node { get; private set; }

        public static HtmlNode CreateNode(string name, Dictionary<string, string> attributes)
        {
            var node = HtmlNode.CreateNode(string.Format("<{0}></{0}>", name));
            if (attributes != null)
            {

                foreach (var attribute in attributes)
                {
                    node.SetAttributeValue(attribute.Key, attribute.Value);
                }
            }
            return node;
        }

    }

}
