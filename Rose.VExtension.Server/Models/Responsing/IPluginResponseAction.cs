using System.Data.Entity;
using System.Globalization;
using System.Xml.Linq;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.Server.Models.Responsing
{
    public interface IPluginResponseAction<out T>
    {
        T Create(PluginResponse response);
    }

    public class PluginResponseXml : IPluginResponseAction<XDocument>
    {
        public XDocument Create(PluginResponse response)
        {
            var xml = new XDocument();
            var root = new XElement("answer");
            xml.Add(root);
            var code = new XElement("code");
            code.Value = ((int)response.ResponseCode).ToString(CultureInfo.InvariantCulture);
            root.Add(code);


            if (response.ResponseCode == ResponseCodeType.Redirect)
            {
                var redirect = new XElement("redirect");
                var to = response.Data[PluginResponse.ResponseDataRedirect];
                if (to != null)
                {
                    redirect.Value = to.ToString();
                }
                root.Add(redirect);
            }

            if (response.ResponseCode == ResponseCodeType.Error)
            {
                var error = new XElement("error");
                var message = response.Data[PluginResponse.ResponseDataErrorMessage];
                error.Value = message.ToString();
                root.Add(error);
            }

            var text = System.Security.SecurityElement.Escape(xml.ToString());
            return xml;
        }
    }

}
