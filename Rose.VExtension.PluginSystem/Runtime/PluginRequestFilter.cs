namespace Rose.VExtension.PluginSystem.Runtime
{
    public class PluginRequestFilter : IPluginRequestFilter
    {
        public PluginRequestFilter(string action)
        {
            Action = action;
        }

        public string Action { get; private set; }

        public bool IsValidRequest(PluginRequest request)
        {
            return true;
        }
    }
}
