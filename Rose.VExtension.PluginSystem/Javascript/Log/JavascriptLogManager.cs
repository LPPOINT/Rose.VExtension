namespace Rose.VExtension.PluginSystem.Javascript.Log
{
    public static class JavascriptLogManager
    {
        public static IJavascriptLogger GetCurrentLogger()
        {
            return new JavascriptNLogger();
        }
    }
}
