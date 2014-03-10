using System;

namespace Rose.VExtension.Server.SettingsVisualization
{
    public interface ISettingsControlVisualizer
    {
        Type ControlType { get; }
        string ControlView { get; }
    }
}
