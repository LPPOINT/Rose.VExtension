using System;
using NLog;

namespace Rose.VExtension.PluginSystem.Common
{
    public static class ExceptionExtensions
    {
        public static void MakeLogReport(this Exception e)
        {
            LogManager.GetCurrentClassLogger().ErrorException("Возникла ошибка во время выполнения программы", e);
        }
    }
}
