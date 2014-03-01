using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Web.Query.Dynamic;
using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Common;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Runtime
{

    public enum ResponseCodeType
    {
        Html = 0,
        Redirect = 1,
        None = 2,
        Error = 3
    }

    public class ResponseInjectionException : Exception
    {
        public ResponseInjectionException()
        {
        }

        public ResponseInjectionException(string message) : base(message)
        {
        }

        protected ResponseInjectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ResponseInjectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class PluginResponseContext
    {
        public PluginResponseContext(PluginResponse response)
        {
            Response = response;
        }

        public PluginResponse Response { get; private set; }
    }

    public class PluginResponse
    {

        public static readonly string ResponseDataRedirect = "Redirect";
        public static readonly string ResponseDataErrorMessage = "Error";

        public PluginResponse()
        {
            Data = new Dictionary<string, object>();
            ResponseCode = ResponseCodeType.Html;
        }

        public IDictionary<string, object> Data { get; private set; }
        public ResponseCodeType ResponseCode { get; set; }

        /// <summary>
        /// Определяет действие перехода по указанному url
        /// </summary>
        public void Redirect(string url)
        {
            
        }

        /// <summary>
        /// Определяет действие изменения html-кода текущей страницы
        /// </summary>
        /// <param name="document"></param>
        public void Html(HtmlDocument document)
        {
            
        }

        public T CreateContext<T>() where T : PluginResponseContext
        {
            try
            {
                return Activator.CreateInstance(typeof(T), this) as T;
            }
            catch 
            {
                return null;
            }
        }

        public dynamic CreateDynamicContext()
        {
            return GetDynamicObject(Data);
        }

        private static dynamic GetDynamicObject(IEnumerable<KeyValuePair<string, object>> properties)
        {
            var dynamicObject = new ExpandoObject() as IDictionary<string, Object>;
            foreach (var property in properties)
            {
                dynamicObject.Add(property.Key, property.Value);
            }
            return dynamicObject;
        }

    }
}
