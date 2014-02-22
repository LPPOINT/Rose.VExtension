namespace Rose.VExtension.PluginSystem.UserSettings
{
    [ControlName("Switch")]
    public class SwitchSettingsControl : ISettingsControl, ITitleableSettingsControl, IValueableSettingsControl<bool>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool Value { get; set; }
    }
}
