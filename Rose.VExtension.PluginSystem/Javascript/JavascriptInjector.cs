using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Noesis.Javascript;
using Rose.VExtension.PluginSystem.Javascript.Log;
using Rose.VExtension.PluginSystem.Javascript.Wrappers;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Javascript
{


    [Serializable]
    public class JavascriptInjectionException : Exception
    {


        public JavascriptInjectionException()
        {
        }

        public JavascriptInjectionException(string message) : base(message)
        {
        }

        public JavascriptInjectionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected JavascriptInjectionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// Внедряет оболочки .NET объектов в JavascriptContext
    /// </summary>
    public class JavascriptInjector
    {
        public JavascriptInjector(JavascriptContext context)
        {
            Context = context;
            InitializeWrappers();
        }

        private Dictionary<Type, Type> wrappers;

        private void InitializeWrappers()
        {
            wrappers = new Dictionary<Type, Type>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.GetInterfaces().Contains(typeof (IJavascriptWrapper)) && !type.IsAbstract)
                {
                    var attrubute = type.GetCustomAttribute<WrapperAttribute>();
                    if (attrubute != null && !wrappers.ContainsKey(attrubute.SourceType))
                    {
                        wrappers.Add(attrubute.SourceType, type);
                    }
                }
            }
        }

        public JavascriptContext Context { get; private set; }

        private void CreateInjection(string name, object obj)
        {
            Context.SetParameter(name, obj);
        }
        private object WrapObject(object obj)
        {
            try
            {
                var objType = obj.GetType();

                if (objType.GetCustomAttribute<NotWrappedAttribute>() != null)
                    return obj;

                var wrapperType = wrappers[objType];
                var wrapper = Activator.CreateInstance(wrapperType, obj);

                return wrapper;

            }
            catch 
            {
                return null;
            }

        }
        private void WrapAndCreateInjection(string name, object obj)
        {
            var wrapped = WrapObject(obj);

            if (wrapped == null)
            {
                throw new JavascriptInjectionException("Не удалось создать javascript-оболочку для заданного объекта");
            }

            CreateInjection(name, wrapped);

        }
        public  void InjectPlugin(Plugin plugin)
        {
            WrapAndCreateInjection(JavascriptConstatns.PluginParametr, plugin);
        }
        public void InjectLogger(IJavascriptLogger logger)
        {
            WrapAndCreateInjection(JavascriptConstatns.LogParametr, logger);
        }
        public void InjectRequest(PluginRequest request)
        {
            WrapAndCreateInjection(JavascriptConstatns.RequestParametr, request);
        }
        public void InjectResponse(PluginResponse response)
        {
            WrapAndCreateInjection(JavascriptConstatns.ResponseParametr, response);
        }
    }
}