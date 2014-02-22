using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Rose.VExtension.PluginSystem.Activation
{

    /// <summary>
    /// Возникает при ошибки во время активации плагина
    /// </summary>
    public class ActivationStepException : Exception
    {

        /// <summary>
        /// Имя шага, во время которого произошла ошибка
        /// </summary>
        public ActivationStepName StepName { get; private set; }

        /// <summary>
        /// Флаг, указывающий, следует ли прекратить дальнейшую активацию плагина после возниктовения исключения
        /// </summary>
        public bool IsFatal { get; private set; }

        public ActivationStepException()
            : base("Во время выполнения шага активации произошла ошибка")
        {
            
        }

        public ActivationStepException(string message) : base(message)
        {
            
        }

        public ActivationStepException(bool isFatal)
            : base("Во время выполнения шага активации произошла ошибка")
        {
            IsFatal = isFatal;
        }

        public ActivationStepException(ActivationStepName stepName, bool isFatal = false): base(String.Format("Во время выполнения шага активации '{0}' произошла ошибка", stepName))
        {
            IsFatal = isFatal;
            StepName = stepName;
        }

        public ActivationStepException(string message, ActivationStepName stepName, bool isFatal = false)
            : base(message)
        {
            IsFatal = isFatal;
            StepName = stepName;
        }

        public ActivationStepException(string message, Exception innerException, ActivationStepName stepName, bool isFatal = false)
            : base(message, innerException)
        {
            IsFatal = isFatal;
            StepName = stepName;
        }
    }

    public class ActivationOrder : IEnumerable<ActivationStepName>
    {

        public ActivationOrder(params ActivationStepName[] names)
        {
            this.names = new List<ActivationStepName>(names);
        }

        public ActivationOrder()
        {
            names = new List<ActivationStepName>();
        }

        private readonly List<ActivationStepName> names;

        public bool Add(ActivationStepName name)
        {

            if(names.Contains(name))
                return false;
            names.Add(name);
            return true;
        }

        public void Insert(ActivationStepName name, int index)
        {
            names.Insert(index, name);
        }
        public void Remove(int index)
        {
            names.RemoveAt(index);
        }

        public static ActivationOrder CreateDefault()
        {
            var namesStr = Enum.GetNames(typeof (ActivationStepName));
            var names = new List<ActivationStepName>();

            foreach (var s in namesStr)
            {
                ActivationStepName res;
                var isParsed = Enum.TryParse(s, out res);
                if (isParsed)
                {
                    names.Add(res);
                }
            }
            return new ActivationOrder(names.ToArray());

        }

        public IEnumerator<ActivationStepName> GetEnumerator()
        {
            return names.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public interface IActivationStepService
    {
        void AddStepProvider(object provider);
        void Activate(Plugin plugin, ActivationInfo info, ActivationOrder order);
    }

    public class ActivationStepService : IActivationStepService
    {

        public ActivationStepService()
        {
            providers = new List<object>();
        }

        private readonly List<object> providers; 

        public void AddStepProvider(object provider)
        {
            providers.Add(provider);
        }

        public object GetProviderForStep(ActivationStepName name)
        {
            foreach (object provider in providers)
            {
                var methods = provider.GetType().GetMethods();

                foreach (var methodInfo in methods)
                {
                    if (methodInfo.GetCustomAttribute<ActivationStepAttribute>() != null &&
                        methodInfo.GetCustomAttribute<ActivationStepAttribute>().Name == name)
                        return provider;
                }


            }
            return null;
        }

        public Action<Plugin, ActivationInfo> GetActivationActionForStep(ActivationStepName name)
        {
            var provider = GetProviderForStep(name);
            if (provider == null)
                return null;

            var methods = provider.GetType().GetMethods();

            foreach (var methodInfo in methods)
            {
                if (methodInfo.GetCustomAttribute<ActivationStepAttribute>() != null &&
                    methodInfo.GetCustomAttribute<ActivationStepAttribute>().Name == name)
                {

                    Action<Plugin, ActivationInfo> action = (plugin, info) => methodInfo.Invoke(provider, new object[] { plugin, info });
                    return action;
                }
            }

                return null;



        }

        public void Activate(Plugin plugin, ActivationInfo info,  ActivationOrder order)
        {
            info.ActivationHandler.OnActivationStarted();
            foreach (var step in order)
            {
                var action = GetActivationActionForStep(step);
                if (action == null)
                    info.ActivationHandler.OnException(new ActivationStepException(String.Format("Не найден обработчик шага '{0}'", step), step));

                try
                {
                    action(plugin, info);
                    info.ActivationHandler.OnStepComplite(new ActivationStepCompliteEventArgs(step));
                }
                catch (ActivationStepException e)
                {
                    info.ActivationHandler.OnException(e);
                    if (e.IsFatal)
                        return;
                }
                catch (Exception e)
                {

                    if (e.InnerException is ActivationStepException)
                    {
                        var innerActivationException = e.InnerException as ActivationStepException;
                        info.ActivationHandler.OnException(innerActivationException);
                    }
                    else
                    {
                        var activationException = new ActivationStepException(e.Message, step);
                        info.ActivationHandler.OnException(activationException);
                    }
                }

            }
            info.ActivationHandler.OnActivationEnded();
        }
    }

}
