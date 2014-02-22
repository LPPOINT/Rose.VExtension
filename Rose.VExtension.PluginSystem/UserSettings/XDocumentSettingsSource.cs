using System.Linq;
using System.Xml.Linq;

namespace Rose.VExtension.PluginSystem.UserSettings
{
    public class XDocumentSettingsSource : ISettingsSource
    {
        public XDocumentSettingsSource(XDocument xml)
        {
            XML = xml;
        }

        public XDocument XML { get; private set; }

        public UserSettingsCollection GetSettings()
        {
            var controls = XML.Root.Elements();
            var activator = new SettingsControlActivator();
            var result = new UserSettingsCollection();

            result.AddRange(controls.Select(activator.Activate).Where(control => control != null));

            return result;

        }
    }
}
