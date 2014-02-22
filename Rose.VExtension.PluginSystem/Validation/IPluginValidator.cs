using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Validation
{
    public interface IPluginValidator
    {
        void ValidatePluginName(string name);
        void ValidatePluginVersion(string version);
    }
}
