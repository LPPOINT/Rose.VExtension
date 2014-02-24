using System.Drawing;
using System.IO;
using System.Linq;
using Rose.VExtension.PluginSystem.Common;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Resources
{
    public interface IPluginLogoProvider
    {

        Plugin Plugin { get; }

        Bitmap CreateBitmap();

        Stream GetLogoStream();
    }

    public class PluginLogoProvider : IPluginLogoProvider
    {
        public PluginLogoProvider(Plugin plugin)
        {

            Check.NotNull(plugin);

            Plugin = plugin;
        }

        public Plugin Plugin { get; private set; }

        public Bitmap CreateBitmap()
        {
            using (var logoStream = GetLogoStream())
            {
                var bmp = new Bitmap(logoStream);
                return bmp;
            }
        }

        public Stream GetLogoStream()
        {
            Check.NotNull(Plugin.FileSystem);
            Check.NotNull(Plugin.PluginConfiguration);

            var logoFilePathSection =
                Plugin.PluginConfiguration.GetItem("Manifest/").Content.FirstOrDefault(item => item.Key == "Logo");
            var logoFilePath = logoFilePathSection.Value ?? FileSystemItem.GetLogoItem().Uri;
            return Plugin.FileSystem.GetItemStream(new FileSystemItem(logoFilePath));

        }

        public bool IsLogoPathDeclared
        {
            get { return Plugin.PluginConfiguration.GetItem("Manifest").Content.Any(pair => pair.Key == "Logo"); }
        }

    }

}
