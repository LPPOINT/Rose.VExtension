using NLog;
using Rose.VExtension.PluginSystem.Activation;

namespace Rose.VExtension.Server.Models.Transactions
{
    /// <summary>
    /// Обработчик активации, записывающий сообщения об активации в заданный логгер
    /// </summary>
    public class LogInitializationHandler : PluginInitializationHandler
    {
        public LogInitializationHandler(Logger logger)
        {
            Logger = logger;
        }

        public Logger Logger { get; private set; }

        public override void OnActivationStarted()
        {
            base.OnActivationStarted();
            Logger.Info("Активация плагина начата");
        }

        public override void OnActivationEnded()
        {
            base.OnActivationEnded();
            Logger.Info("Активация плагина завершена");
        }

        public override void OnStepComplite(ActivationStepCompliteEventArgs eventArgs)
        {
            base.OnStepComplite(eventArgs);
            Logger.Info("Завершено выполнение шага активации '" + eventArgs.StepName + "'.");
        }

        public override void OnException(ActivationStepException exception)
        {
            base.OnException(exception);
            Logger.ErrorException("Обнаружена ошибка активации", exception);
        }
    }
}