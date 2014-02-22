namespace Rose.VExtension.PluginSystem.Javascript.Wrappers
{

    [Wrapper(typeof(Plugin))]
    public class PluginWrapper : JavascriptWrapper
    {
        public PluginWrapper(Plugin source) : base(source)
        {
        }
    }
}
