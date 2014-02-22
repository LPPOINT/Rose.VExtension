﻿using System;
using System.Runtime.Serialization;
using NLog;
using NLog.Targets;
using Noesis.Javascript;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.Activation.Platforms;
using Rose.VExtension.PluginSystem.Javascript;
using Rose.VExtension.PluginSystem.Javascript.Log;

namespace Rose.VExtension.PluginSystem.Runtime
{
    [Serializable]
    public class JavascriptDomainException : Exception
    {
        public JavascriptDomainException()
        {
        }

        public JavascriptDomainException(string message) : base(message)
        {
        }

        public JavascriptDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JavascriptDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class JavascriptDomain : PluginDomain
    {

        public override PluginResponse Execute(PluginRequest request)
        {



            using (var context = new JavascriptContext())
            {

                var response = new PluginResponse();

                var sourceProvider = new JavascriptSourceProvider();
                var script = sourceProvider.GetSources(Plugin, Plugin.Platform as JSPluginPlatform);

                if (string.IsNullOrWhiteSpace(script))
                {
                    throw new JavascriptDomainException("Не удалось составить javascript-код плагина");
                }

                var platform = Plugin.Platform as JSPluginPlatform;
                var logger = JavascriptLogManager.GetCurrentLogger();

                try
                {

                    var injector = context.CreateInjector();

                    injector.InjectPlugin(Plugin);
                    injector.InjectLogger(logger);
                    injector.InjectRequest(request);
                    injector.InjectResponse(response);


                }
                catch (Exception e)
                {
                    throw new JavascriptDomainException("Ошибка внедрения переменных в javascript-код плагина", e);
                }

                try
                {
                    context.Run(script);
                    context.Run(String.Format("{0}();", platform.EntryFunction));

                    return response;
                }
                catch (Exception e)
                {
                    throw new JavascriptDomainException("Ошибка выполнения javascript-кода плагина", e);
                }

            }

        }
    }
}