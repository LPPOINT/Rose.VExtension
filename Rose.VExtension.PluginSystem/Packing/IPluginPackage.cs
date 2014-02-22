using System.IO;
using Rose.VExtension.PluginSystem.Activation;

namespace Rose.VExtension.PluginSystem.Packing
{
    public interface IPluginPackage
    {
        PacckageType Type { get;  }
        void FromStream(Stream stream);
        Stream GetStream();
    }
}
