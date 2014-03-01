using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.PluginSystem.Runtime.RequestHandeling;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Models.Transactions;

namespace Rose.VExtension.Server.Models
{

    public class PluginExecutionException : Exception
    {
        public PluginExecutionException()
        {
        }

        public PluginExecutionException(string message) : base(message)
        {
        }

        public PluginExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class PluginExecutionResult
    {
        public PluginExecutionResult(PluginResponse response, TimeSpan executionDuration)
        {
            ExecutionDuration = executionDuration;
            Response = response;
        }

        public PluginResponse Response { get; private set; }
        public TimeSpan ExecutionDuration { get; private set; }
    }

    public interface IPluginsExecutor
    {
        PluginExecutionResult ExecutePlugin(PluginRequestModel request, string pluginId);
    }

    public interface IPluginAsyncExecutor
    {
        Task<PluginExecutionResult> ExecutePluginAsync(PluginRequestModel request, string pluginId);
    }

    public class PluginsExecutor : IPluginsExecutor, IPluginAsyncExecutor
    {
        public PluginsExecutor(IPluginTransactor transactor, IPluginsRepository repository, IPluginsCollection plugins)
        {
            Plugins = plugins;
            Repository = repository;
            Transactor = transactor;
        }

        public PluginsExecutor(PluginsControllerContext context) : this(context.Transactor, context.Repository, context.Plugins)
        {
            
        }

        /// <summary>
        /// Предоставляет методы транзакции для выполнения плагина
        /// </summary>
        public IPluginTransactor Transactor { get; private set; }
        /// <summary>
        /// Предоставляет доступ к базе данных плагинов
        /// </summary>
        public IPluginsRepository Repository { get; private set; }
        /// <summary>
        /// Предоставляет доступ к загруженным плагинам
        /// </summary>
        public IPluginsCollection Plugins { get; private set; }

        private PluginExecutionResult Run(PluginRequestModel request,  PluginSystem.Plugin plugin)
        {
            var handlers = plugin.Controller.GetHandlersForRequest(request.CreatePluginRequest());
            var firstHandler = handlers.FirstOrDefault();

            if(firstHandler == null)
                return new PluginExecutionResult(null, new TimeSpan());

            firstHandler.Arguments.Source = new PluginArgumentsSource(plugin, request, request.CreatePluginRequest());

            var response = firstHandler.Activity.Execute(request.CreatePluginRequest());

            firstHandler.Arguments.Source = null;

            return new PluginExecutionResult(response, new TimeSpan());


        }

        public PluginExecutionResult ExecutePlugin(PluginRequestModel request, string pluginId)
        {
            var status = Repository.GetPluginStatus(pluginId);

            if (status == PluginLocation.InRam)
            {
                var connection = PluginConnection.GetConnection(pluginId);
                if(connection == null)
                    throw new PluginExecutionException();
                var plugin = connection.ControlPlugin;
                return Run(request, plugin);
            }
            throw new NotImplementedException();
        }

        public async Task<PluginExecutionResult> ExecutePluginAsync(PluginRequestModel request, string pluginId)
        {
            return await Task.Run(() => ExecutePlugin(request, pluginId));
        }
    }

}
