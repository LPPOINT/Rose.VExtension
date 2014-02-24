using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Rose.VExtension.PluginSystem.Runtime;
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
        PluginExecutionResult ExecutePlugin(PluginRequest request, string pluginId);
    }

    public interface IPluginAsyncExecutor
    {
        Task<PluginExecutionResult> ExecutePluginAsync(PluginRequest request, string pluginId);
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

        private PluginExecutionResult Run(PluginRequest request, PluginSystem.Plugin plugin)
        {


            return null;
        }

        public PluginExecutionResult ExecutePlugin(PluginRequest request, string pluginId)
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

        public async Task<PluginExecutionResult> ExecutePluginAsync(PluginRequest request, string pluginId)
        {
            return await Task.Run(() => ExecutePlugin(request, pluginId));
        }
    }

}
