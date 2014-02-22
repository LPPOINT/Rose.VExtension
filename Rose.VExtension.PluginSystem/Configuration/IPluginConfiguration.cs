namespace Rose.VExtension.PluginSystem.Configuration
{
    public interface IPluginConfiguration
    {
        IConfigurationItem RootItem { get; }

        IConfigurationItem GetItem(string path);
        string GetItemValue(string path);
        bool ContainsItem(string path);

    }

}
