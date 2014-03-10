using System;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.Serialization;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.Server.Responsing
{
    public interface IResponseMessager
    {
        string Create(PluginResponse response);
        string CreateEmpty();
        string CreateException(Exception exception);
    }

    public class XmlResponseMessager : IResponseMessager
    {
        public string Create(PluginResponse response)
        {
            var xml = new XDocument();
            var root = new XElement("answer");
            xml.Add(root);
            var code = new XElement("code")
                       {
                           Value =
                               ((int) response.ResponseCode).ToString(CultureInfo.InvariantCulture)
                       };
            root.Add(code);

            var context = PluginResponseContext.Create(response);
            if(context != null)
                context.SerializeTo(root);

            return xml.ToString();
        }

        public string CreateEmpty()
        {
            var res = new PluginResponse {ResponseCode = ResponseCodeType.None};
            return Create(res);
        }

        public string CreateException(Exception exception)
        {
            var xml = new XDocument();
            var root = new XElement("answer");
            xml.Add(root);
            var code = new XElement("code")
            {
                Value = "0"
            };
            root.Add(code);

            var ser = new XmlSerializer(exception.GetType());
            ser.Serialize(root.CreateWriter(), exception);

            return xml.ToString();
        }
    }

}
