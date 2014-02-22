using Noesis.Javascript;

namespace Rose.VExtension.PluginSystem.Javascript
{
    public static class JavascriptContextExtensions
    {
        public static JavascriptInjector CreateInjector(this JavascriptContext context)
        {
            return new JavascriptInjector(context);
        }
    }
}
