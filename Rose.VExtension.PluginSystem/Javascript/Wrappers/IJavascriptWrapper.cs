namespace Rose.VExtension.PluginSystem.Javascript.Wrappers
{
    public interface IJavascriptWrapper
    {
        object Source { get; }
    }

    public class JavascriptWrapper : IJavascriptWrapper
    {
        public JavascriptWrapper(object source)
        {
            Source = source;
        }

        public object Source { get; private set; }
    }

}
