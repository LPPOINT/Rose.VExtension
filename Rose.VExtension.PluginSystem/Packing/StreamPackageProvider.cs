using System.IO;

namespace Rose.VExtension.PluginSystem.Packing
{
    public class StreamPackageProvider : IPluginPackageProvider
    {
        public StreamPackageProvider(Stream stream)
        {
            Stream = stream;
        }


        public Stream Stream { get; private set; }

        public IPluginPackage GetPackage()
        {
            return new StreamPluginPackage(Stream);
        }
    }
}
