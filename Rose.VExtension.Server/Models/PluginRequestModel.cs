using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.Models
{

    public class RequestParsingException : Exception
    {
        public RequestParsingException()
        {
        }

        public RequestParsingException(string message) : base(message)
        {
        }

        public RequestParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RequestParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    /// <summary>
    /// Представляет запрос передаваемый от клиентского расширения браузера к серверу
    /// </summary>
    public class PluginRequestModel : IPluginRequest
    {
        public string MetaString { get; set; }
        public IDictionary<string, string> Headers
        {
            get
            {
                try
                {
                    var kv = MetaString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    var res = new Dictionary<string, string>();

                    foreach (var s in kv)
                    {
                        try
                        {
                            var kvf = s.Trim().Split(new []{'='}, StringSplitOptions.RemoveEmptyEntries);

                            if (kvf.Length == 2)
                            {
                                var name = kvf[0];
                                var value = kvf[1];
                                res.Add(name, value);
                            }
                        }
                        catch 
                        {
                        
                        }

                    }

                    return res;
                }
                catch 
                {
                    return new Dictionary<string, string>();
                }

            }
        }

        /// <summary>
        /// Идентификатор пользователя ВКонтакте
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор расширения пользователя
        /// </summary>
        public string ExtensionId { get; set; }

        /// <summary>
        /// Токен авторизации пользователя ВКонтакте
        /// </summary>
        public string UserToken { get; set; }

        public string RequestString { get; set; }

        /// <summary>
        /// URL-адрес, на котором находился пользователь при отправке запроса
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// HTML вкладки, открытой пользователем при выполнении запроса
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Максимальное время, данное на выполнение плагина
        /// </summary>
        public string Timeout { get; set; }

        /// <summary>
        /// Строка аттрибутов, заданная при запросе
        /// </summary>
        public string AttributesString { get; set; }

        /// <summary>
        /// Строка аттрибутов, разделенная запятой
        /// </summary>
        public IEnumerable<string> Attributes
        {
            get { return AttributesString.Split(','); }
        }

        public PluginRequest CreatePluginRequest()
        {
            var result = new PluginRequest
                         {

                             Headers = Headers,
                             RequestString = RequestString,

                         };


            return result;
        }


        public static PluginRequestModel ParseXml(string xml)
        {
            var doc = XDocument.Parse(xml);

            if (doc.Root.Name != "PluginRequestModel")
            {
                doc.Root.Name = "PluginRequestModel";
            }

            var serializer = new XmlSerializer(typeof (PluginRequestModel));
            var requestModel = serializer.Deserialize(doc.CreateReader());
            return requestModel as PluginRequestModel;

        }

        public PluginRequestModel Parse(string inputBody, string contentType)
        {
            if (contentType.ToLower() == "text/xml" || contentType.ToLower() == "text/html")
                return ParseXml(inputBody);
            throw new RequestParsingException("Неопознанный contentType");
        }

        public static PluginRequestModel CreateDefault()
        {
            var request = new PluginRequestModel
                          {
                              AttributesString = "attribute1, attribute2",
                              ExtensionId = "21315534",
                              Html = "<html><head><title>test request</title></head></html>",
                              MetaString = "name=value, name2=value2",
                              RequestString = "TestPluginName/TestPluginAction/OtherComponents",
                              Timeout = "10000",
                              UserId = "244325243",
                              UserToken = Guid.NewGuid().ToString()
                          };

            return request;

        }



    }
}