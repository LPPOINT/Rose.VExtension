using System.Collections.Generic;

namespace Rose.VExtension.PluginSystem.UserSettings
{
    [ControlName("List")]
    public class ListSettingsControl : ISettingsControl, ITitleableSettingsControl, IValueableSettingsControl<string>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; } 
    }
}
