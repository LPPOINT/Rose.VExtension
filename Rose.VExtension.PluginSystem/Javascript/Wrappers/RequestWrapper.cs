using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Javascript.Wrappers
{
    [Wrapper(typeof(PluginRequest))]
    public class RequestWrapper : JavascriptWrapper
    {
        public RequestWrapper(PluginRequest source) : base(source)
        {
        }
    }
}
