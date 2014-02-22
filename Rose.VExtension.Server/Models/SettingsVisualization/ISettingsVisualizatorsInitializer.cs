using System.Collections.Generic;

namespace Rose.VExtension.Server.Models.SettingsVisualization
{
    public interface ISettingsVisualizatorsInitializer
    {
        void Initialize(ICollection<ISettingsControlVisualizer> visualizers);
    }
}
