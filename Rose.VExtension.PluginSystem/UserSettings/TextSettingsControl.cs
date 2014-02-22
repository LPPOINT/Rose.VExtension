namespace Rose.VExtension.PluginSystem.UserSettings
{
    [ControlName("Text")]
    public class TextSettingsControl : ISettingsControl,
        ITitleableSettingsControl,
        IValueableSettingsControl<string>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
