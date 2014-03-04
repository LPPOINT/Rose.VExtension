using System;
using System.Runtime.Serialization;
using HtmlAgilityPack;
using Noesis.Javascript;
using Rose.VExtension.PluginSystem.Activation.RuntimeActivation;
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

    public class JavascriptActivity : PluginActivity
    {

        public override PluginResponse Execute(PluginRequest request)
        {



            using (var context = new JavascriptContext())
            {

                var response = new PluginResponse();
                var sourceProvider = new JavascriptSourceProvider();
                var script = sourceProvider.GetSources(Plugin, Platform as JSPluginPlatform);

                if (string.IsNullOrWhiteSpace(script))
                {
                    throw new JavascriptDomainException("Не удалось составить javascript-код плагина");
                }

                var platform = Platform as JSPluginPlatform;
                var logger = JavascriptLogManager.GetCurrentLogger();

                context.SetParameter("log", logger);
                context.SetParameter("request", request);
                context.SetParameter("response", response);
                context.SetParameter("plugin", Plugin);
                context.SetParameter("args", Handler.Arguments.Source);

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
