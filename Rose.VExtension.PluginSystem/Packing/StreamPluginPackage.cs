using System.IO;
using Rose.VExtension.PluginSystem.Activation;

namespace Rose.VExtension.PluginSystem.Packing
{
    public class StreamPluginPackage : IPluginPackage
    {
        public StreamPluginPackage(Stream stream)
        {
            Stream = stream;
            Type = PacckageType.ZipFile;
        }

        public Stream Stream { get; private set; }

        public override int GetHashCode()
        {
            return Stream.GetHashCode();
        }

        public PacckageType Type { get; private set; }
        public void FromStream(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public Stream GetStream()
        {
            return Stream;
        }
    }
}
