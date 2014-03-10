using System.Collections.Generic;

namespace Rose.VExtension.Server.SettingsVisualization
{
    public interface ISettingsVisualizatorsInitializer
    {
        void Initialize(ICollection<ISettingsControlVisualizer> visualizers);
    }
}
