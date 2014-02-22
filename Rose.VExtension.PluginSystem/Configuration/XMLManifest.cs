using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Rose.VExtension.PluginSystem.Configuration
{

    public class XMLManifestParseException : ManifestParseException
    {
        public XMLManifestParseException(string message) : base(message)
        {
        }

        public XMLManifestParseException(XmlException xmlException)
        {
            XmlException = xmlException;
        }

        public XMLManifestParseException(string message, XmlException xmlException) : base(message)
        {
            XmlException = xmlException;
        }

        public XMLManifestParseException(string message, Exception innerException, XmlException xmlException) : base(message, innerException)
        {
            XmlException = xmlException;
        }

        public XmlException XmlException { get; private set; }
    }

    /// <summary>
    /// Представляет конфигурацию, описанную на языке разметка XML
    /// </summary>
    public class XMLManifest : IManifest
    {
        public XMLManifest(XDocument xml)
        {
            XML = xml;
        }

        public XDocument XML { get; private set; }

        private IConfigurationItem CreateConfigurationItemTree(XElement node, IPluginConfiguration config)
        {
            var name = node.Name.ToString();

            if(!PluginConfiguration.IsValidItemName(name))
                throw new XMLManifestParseException("Ошибка в написании имени узла конфигурации");

            var item = new ConfigurationItem(name);
            var attribs = node.Attributes();
            var childs = node.Elements();

            item.PluginConfiguration = config;

            foreach (var xAttribute in attribs)
            {
                item.Content.Add(xAttribute.Name.ToString(), xAttribute.Value);
            }

            foreach (var xElement in childs)
            {
                if (!xElement.HasElements)
                {

                    var itemName = xElement.Name.ToString();
                    var itemValue = xElement.Value;
                    //if(!PluginConfiguration.IsValidItemName(itemName) || !PluginConfiguration.IsValidItemValue(itemValue))
                    //    throw new XMLManifestParseException("Ошибка в написании имени или значения узла конфигурации");

                    item.Content.Add(itemName, itemValue);
                }
                else
                {
                    var childItem = CreateConfigurationItemTree(xElement, config);
                    childItem.Parent = item;
                    item.InnerItems.Add(childItem);
                }
            }

            return item;
        }

        public IPluginConfiguration CreateConfiguration()
        {
            var config = new PluginConfiguration();
            config.RootItem = CreateConfigurationItemTree(XML.Root, config);
            return config;
        }
    }
}
