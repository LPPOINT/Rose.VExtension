using NLog;


namespace Rose.VExtension.PluginSystem.Javascript.Log
{

    public class JavascriptNLogger : IJavascriptLogger
    {
        public void Trace(string message)
        {
            LogManager.GetCurrentClassLogger().Trace(message);
        }

        public void Info(string message)
        {
            LogManager.GetCurrentClassLogger().Info(message);
        }

        public void Warning(string message)
        {
            LogManager.GetCurrentClassLogger().Warn(message);
        }

        public void Error(string message)
        {
            LogManager.GetCurrentClassLogger().Error(message);
        }

        public void Fatal(string message)
        {
            LogManager.GetCurrentClassLogger().Fatal(message);
        }
    }
}
