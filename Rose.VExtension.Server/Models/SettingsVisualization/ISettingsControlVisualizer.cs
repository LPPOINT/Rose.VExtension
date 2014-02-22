using System;

namespace Rose.VExtension.Server.Models.SettingsVisualization
{
    public interface ISettingsControlVisualizer
    {
        Type ControlType { get; }
        string ControlView { get; }
    }
}
