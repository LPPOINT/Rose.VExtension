using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.Models
{

    /// <summary>
    /// Представляет запрос передаваемый от клиентского расширения браузера к серверу
    /// </summary>
    public class PluginRequestModel : IPluginRequest
    {
        public string MetaString { get; set; }
        public IDictionary<string, string> Meta
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
                             Attributes = Attributes,
                             ExtensionId = ExtensionId,
                             Meta = Meta,
                             RequestString = RequestString,
                             Url = Url,
                             UserId = UserId,
                             Html = new HtmlDocument()
                         };

            result.Html.LoadHtml(Html);

            return result;
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