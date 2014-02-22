using System;

namespace Rose.VExtension.Server.Models.SettingsVisualization
{
    public class SettingsControlVisualizer : ISettingsControlVisualizer
    {
        public SettingsControlVisualizer(Type controlType, string controlView)
        {
            ControlView = controlView;
            ControlType = controlType;
        }

        public Type ControlType { get; private set; }
        public string ControlView { get; private set; }
    }
}