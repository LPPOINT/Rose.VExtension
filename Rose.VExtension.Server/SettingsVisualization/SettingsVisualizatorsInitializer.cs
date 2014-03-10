using System.Collections.Generic;
using Rose.VExtension.PluginSystem.UserSettings;

namespace Rose.VExtension.Server.SettingsVisualization
{
    public class SettingsVisualizatorsInitializer : ISettingsVisualizatorsInitializer
    {

        private static string GetViewPath(string viewName, bool withExtension = true)
        {
            var result =  "~/Areas/Plugins/Views/Shared/SettingsVisualizers/" + viewName;

            if (withExtension)
                result += ".cshtml";

            return result;

        }

        public void Initialize(ICollection<ISettingsControlVisualizer> visualizers)
        {
            visualizers.Add(new SettingsControlVisualizer(typeof(SwitchSettingsControl), GetViewPath("SwitchSettingsControlView")));
            visualizers.Add(new SettingsControlVisualizer(typeof(TextSettingsControl), GetViewPath("TextSettingsControlView")));
            visualizers.Add(new SettingsControlVisualizer(typeof(ListSettingsControl), GetViewPath("ListSettingsControlView")));
            visualizers.Add(new SettingsControlVisualizer(typeof(PasswordSettingsControl), GetViewPath("PasswordSettingsControlView")));
        }
    }
}