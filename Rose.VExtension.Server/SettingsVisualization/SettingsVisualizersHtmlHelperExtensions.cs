using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Rose.VExtension.PluginSystem.UserSettings;

namespace Rose.VExtension.Server.SettingsVisualization
{
    public static class SettingsVisualizersHtmlHelperExtensions
    {

        private static readonly ICollection<ISettingsControlVisualizer> visualizers = new List<ISettingsControlVisualizer>();

        public static MvcHtmlString PluginSettings(this HtmlHelper helper, ISettingsControl control)
        {
            if (visualizers.Count == 0)
            {
                var initializer = new SettingsVisualizatorsInitializer();
                initializer.Initialize(visualizers);
            }

            var visual = visualizers.FirstOrDefault(visualizer => visualizer.ControlType == control.GetType());

            if (visual != null)
                return helper.Partial(visual.ControlView, control);
            return null;

        }
    }
}