

namespace Rose.VExtension.PluginSystem.Javascript.Log
{

    public interface IJavascriptLogger
    {
        void Trace(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
    }
}
