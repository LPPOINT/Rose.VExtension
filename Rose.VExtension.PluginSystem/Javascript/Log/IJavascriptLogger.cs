using Rose.VExtension.PluginSystem.Javascript.Wrappers;

namespace Rose.VExtension.PluginSystem.Javascript.Log
{
    [NotWrapped]
    public interface IJavascriptLogger
    {
        void Trace(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
    }
}
