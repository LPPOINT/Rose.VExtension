using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Javascript.Wrappers
{
    [Wrapper(typeof(PluginResponse))]
    public class ResponseWrapper : JavascriptWrapper
    {
        public ResponseWrapper(PluginResponse source)
            : base(source)
        {
        }
    }
}
