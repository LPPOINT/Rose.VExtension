namespace Rose.VExtension.PluginSystem.UserSettings
{
    public interface IValueableSettingsControl<TType>
    {
        TType Value { get; set; }
    }
}
