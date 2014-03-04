using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Common;
using Rose.VExtension.PluginSystem.Runtime.Popups;

namespace Rose.VExtension.PluginSystem.Runtime
{

    /// <summary>
    /// Перечисление кодов ответов
    /// </summary>
    public enum ResponseCodeType
    {
        /// <summary>
        /// Код ответа, при котором будет изменено html-содержимое страницы
        /// </summary>
        Html = 1,
        /// <summary>
        /// Код ответа, при котором браузер должен будет перенаправить клиента по заданной ссылке
        /// </summary>
        Redirect = 2,
        /// <summary>
        /// Код ответа, не выполняющего никаких действий
        /// </summary>
        None = 3,
        /// <summary>
        /// Код ответа, при котором будет оторажен notification-popup элемент
        /// </summary>
        NotificationPopup = 4,
        /// <summary>
        /// Код ответа, при котором будет оторажен window-popup элемент
        /// </summary>
        WindowPopup = 5,
        /// <summary>
        /// Обозначает ответ, при формировании которого произошла ошибка
        /// </summary>
        Error = 0
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

    /// <summary>
    /// При переопределении предоставляет возможность удобного получения данных из ответа и их сериализации
    /// </summary>
    public class PluginResponseContext
    {
        /// <summary>
        /// Инициализиреут заданное свойство текущего экземпляра, значением, взятым из словаря данных ответа с заданным ключем
        /// </summary>
        /// <param name="propertyName">Имя свойства, которое требуется инициализировать</param>
        /// <param name="dataName">Ключ данных ответа</param>
        protected void InitializeProperty(string propertyName, string dataName)
        {
            if (Response == null)
                return;
            var propertyInfo = GetType().GetProperties().FirstOrDefault(info => info.Name == propertyName);
            if(propertyInfo == null)
                return;
            try
            {
                var value = Response.Data[dataName];

                if (value == null)
                    throw new Exception();

                propertyInfo.SetValue(this, value);
            }
            catch 
            {
            }

        }

        public PluginResponseContext(PluginResponse response)
        {
            Response = response;
        }

        /// <summary>
        /// Ответ, от которого должны быть получены данные
        /// </summary>
        [XmlIgnore]
        public PluginResponse Response { get; private set; }

        /// <summary>
        /// Записывает конест в заданный xml узел
        /// </summary>
        /// <param name="element"></param>
        public virtual void SerializeTo(XElement element)
        {
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(element.CreateWriter(), this);
        }

        public static PluginResponseContext Create( PluginResponse response)
        {
            switch (response.ResponseCode)
            {
                case ResponseCodeType.Html:
                    return response.CreateContext<HtmlResponseContext>();
                case ResponseCodeType.Redirect:
                    return response.CreateContext<RedirectResponseContext>();
                case ResponseCodeType.None:
                    return response.CreateContext<EmptyResponseContext>(); 
                case ResponseCodeType.NotificationPopup:
                    return response.CreateContext<NotificationPopupResponseContext>();
                case ResponseCodeType.WindowPopup:
                    return response.CreateContext<HtmlResponseContext>();
                case ResponseCodeType.Error:
                    return response.CreateContext<ErrorResponseContext>();
                default:
                    return null;
            }
        }


    }

    public class HtmlResponseContext : PluginResponseContext
    {
   
        public HtmlDocument Html { get; private set; }

        public override void SerializeTo(XElement element)
        {
            var htmlE = new XElement("html");
            htmlE.Value = Html.DocumentNode.InnerHtml;
            element.Add(htmlE);
        }

        public HtmlResponseContext(PluginResponse response) : base(response)
        {
            InitializeProperty("Html", PluginResponse.ResponseDataHtml);
        }
    }

    public class RedirectResponseContext : PluginResponseContext
    {
        public RedirectResponseContext(PluginResponse response) : base(response)
        {
            InitializeProperty("RedirectUrl", PluginResponse.ResponseDataRedirect);
        }

        public string RedirectUrl { get; private set; }
    }

    public class NotificationPopupResponseContext : PluginResponseContext
    {

        public INotificationPopup NotificationPopup { get; private set; }

        public NotificationPopupResponseContext(PluginResponse response) : base(response)
        {
            InitializeProperty("NotificationPopup", PluginResponse.ResponseDataNotificationPopup);
        }
    }

    public class EmptyResponseContext : PluginResponseContext
    {
        public EmptyResponseContext(PluginResponse response) : base(response)
        {
        }

        public override void SerializeTo(XElement element)
        {
        }
    }

    public class ErrorResponseContext : PluginResponseContext
    {

        public int ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public ErrorResponseContext(PluginResponse response) : base(response)
        {
            InitializeProperty("ErrorCode", PluginResponse.ResponseDataErrorCode);
            InitializeProperty("ErrorMessage", PluginResponse.ResponseDataErrorMessage);
        }
    }

    public class PluginResponse
    {

        public static readonly string ResponseDataRedirect = "Redirect";
        public static readonly string ResponseDataErrorMessage = "Error";
        public static readonly string ResponseDataErrorCode = "ErrorCode";
        public static readonly string ResponseDataHtml = "Html";
        public static readonly string ResponseDataNotificationPopup = "NotificationPopup";
        public static readonly string ResponseDataWindowPopup = "WindowPopup";

        public PluginResponse()
        {
            Data = new Dictionary<string, object>();
            ResponseCode = ResponseCodeType.Html;
        }

        /// <summary>
        /// Данные ответа
        /// </summary>
        public IDictionary<string, object> Data { get; private set; }

        /// <summary>
        /// Код ответа
        /// </summary>
        public ResponseCodeType ResponseCode { get; set; }

        private void SetDataAndResponseCode(ResponseCodeType code, string dataName, object dataValue)
        {
            Check.NotNullOrWhiteSpace(dataName);
            Check.NotNull(dataValue);

            ResponseCode = code;

            Data[dataName] = dataValue;

        }

        /// <summary>
        /// Определяет действие перехода по указанному url
        /// </summary>
        public void Redirect(string url)
        {
            SetDataAndResponseCode(ResponseCodeType.Redirect, ResponseDataRedirect, url);
        }

        /// <summary>
        /// Определяет действие изменения html-кода текущей страницы
        /// </summary>
        /// <param name="document"></param>
        public void Html(HtmlDocument document)
        {
            SetDataAndResponseCode(ResponseCodeType.Html, ResponseDataHtml, document);
        }

        /// <summary>
        /// Определяет действие открытия popup-окна на текущей странице
        /// </summary>
        /// <param name="notificationPopup"></param>
        public void Popup(INotificationPopup notificationPopup)
        {
            SetDataAndResponseCode(ResponseCodeType.NotificationPopup, ResponseDataNotificationPopup, notificationPopup);
        }

        /// <summary>
        /// Создает контест данного ответа, позволяющий облегчить доступ к данным ответа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Создает динамический контест данного ответа
        /// </summary>
        /// <returns></returns>
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
