namespace Rose.VExtension.PluginSystem.Javascript
{
    public interface IJavascriptWrapper<TBase, TJavascript>
    {
        TJavascript Wrap(TBase obj);
    }
}
