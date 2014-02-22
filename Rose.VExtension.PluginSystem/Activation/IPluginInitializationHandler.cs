using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rose.VExtension.PluginSystem.Activation
{

    public class ActivationStepCompliteEventArgs : EventArgs
    {
        public ActivationStepCompliteEventArgs(ActivationStepName stepName)
        {
            StepName = stepName;
        }

        public ActivationStepName StepName { get; private set; }
    }

    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; private set; }
    }

    /// <summary>
    /// Перехватывает события от активатора плагина
    /// </summary>
    public interface IPluginInitializationHandler
    {

        IReadOnlyCollection<ActivationStepName> Steps { get; }
        IReadOnlyCollection<ActivationStepException> Exceptions { get; }

        /// <summary>
        /// Возникает при безошибочном выполнении шага активации
        /// </summary>
        event EventHandler<ActivationStepCompliteEventArgs> StepComplite;

        /// <summary>
        /// Возникает при ошибке во время шага активации. При фатальной ошибке, кроме этого события, возникает событие ActivationFatalException
        /// </summary>
        event EventHandler<ExceptionEventArgs> ActivationException;

        /// <summary>
        /// Возникает при фатальной ошибке во время шага активации. 
        /// </summary>
        event EventHandler<ExceptionEventArgs> ActivationFatalException;

        event EventHandler ActivationStarted;
        event EventHandler ActivationEnded;

        /// <summary>
        /// При переопрделении уаедомляет обработчик об начале активации
        /// </summary>
        void OnActivationStarted();

        /// <summary>
        /// При переопрделении уаедомляет обработчик об завершении активации
        /// </summary>
        void OnActivationEnded();

        /// <summary>
        /// При переопрделении уаедомляет обработчик об безошибочном выполнении шага активации
        /// </summary>
        /// <param name="eventArgs"></param>
        void OnStepComplite(ActivationStepCompliteEventArgs eventArgs);

        /// <summary>
        /// При переопрделении уаедомляет обработчик об ошибочном выполнении шага активации
        /// </summary>
        /// <param name="exception"></param>
        void OnException(ActivationStepException exception);

    }

    public class PluginInitializationHandler : IPluginInitializationHandler
    {
        public PluginInitializationHandler()
        {

            steps = new List<ActivationStepName>();
            exceptions = new List<ActivationStepException>();
        }

        protected readonly List<ActivationStepName> steps;
        protected readonly List<ActivationStepException> exceptions;

        public IReadOnlyCollection<ActivationStepName> Steps
        {
            get
            {
                return new ReadOnlyCollection<ActivationStepName>(steps);
            }
        }
        public IReadOnlyCollection<ActivationStepException> Exceptions
        {
            get
            { return new ReadOnlyCollection<ActivationStepException>(exceptions);}
        }


        public event EventHandler<ActivationStepCompliteEventArgs> StepComplite;
        public event EventHandler<ExceptionEventArgs> ActivationException;
        public event EventHandler<ExceptionEventArgs> ActivationFatalException;
        public event EventHandler ActivationStarted;
        public event EventHandler ActivationEnded;

        public virtual void OnActivationStarted()
        {
            var handler = ActivationStarted;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        public virtual void OnActivationEnded()
        {
            var handler = ActivationEnded;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        public virtual void OnStepComplite(ActivationStepCompliteEventArgs eventArgs)
        {

            steps.Add(eventArgs.StepName);

            var handler = StepComplite;
            if (handler != null) handler(this, eventArgs);
        }
        public virtual void OnException(ActivationStepException exception)
        {
            exceptions.Add(exception);

            var handler = ActivationException;
            if (handler != null) handler(this, new ExceptionEventArgs(exception));

            if (exception.IsFatal)
            {
                var fatalHandler = ActivationFatalException;
                if(fatalHandler  != null) fatalHandler(this, new ExceptionEventArgs(exception));
            }
        }

    }

    public static class PluginInitializationHandlerExtensions
    {
        public static bool ContainsExceptions(this IPluginInitializationHandler handler)
        {
            return handler.Exceptions.Any();
        }

        public static bool ContainsFatalExceptions(this IPluginInitializationHandler handler)
        {
            return handler.Exceptions.Any(exception => exception.IsFatal);
        }


        /// <summary>
        /// На основании поданых для обработчика ошибок активации создает исключение, уведомляющие об этих ошибках
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static PluginInitializationException CreateInitializationException(
            this IPluginInitializationHandler handler)
        {
            return new PluginInitializationException("Во время активации плагина произошла одна илм несколько ошибок. Их список можно посмотреть в свойстве ErrorHandler текущего экземпляра исключения", handler.Exceptions);
        }

    }
}
