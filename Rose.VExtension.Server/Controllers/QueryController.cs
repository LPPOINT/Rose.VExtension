using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.Server.Models;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Models.Responsing;
using Rose.VExtension.Server.Models.Transactions;

namespace Rose.VExtension.Server.Controllers
{
    public class QueryController : Controller
    {


        public QueryController()
        {
            Repository = new PluginsRepository();
            Transactor = new PluginTransactor(Repository);
            Collection = new PluginsCollection();

            Transactor.Out = Collection;
            Transactor.ShouldInspectCollection = true;

        }

        public IPluginTransactor Transactor { get; set; }

        public IPluginsRepository Repository { get; set; }

        public IPluginsCollection Collection { get; set; }

        private IEnumerable<string> GetPluginsForRequest(PluginRequestModel request)
        {
            return (from plugin in Repository.PluginContext.All select plugin.Id).ToList(); // Все плагины в репозитории

        }

        public ActionResult Run()
        {
            var request = PluginRequestModel.CreateDefault();
            var statusProvider = new PluginsPhysicalStatusProvider(Repository, Collection);

            Transactor.Sync(statusProvider);

            var pluginRequest = request.CreatePluginRequest(); // Запрос, отсылаемый к плагину
            var executor = new PluginsExecutor(Transactor, Repository, Collection); // Предоставляет методы для получения ответа от плагина
            var plugins = GetPluginsForRequest(request); // Коллекция идентификаторов плагинов, от которых следует получить ответ 
            var resultBuilder = new RequestResultBuilder(request); // Предоставляет методы для получния конечного ответа от сервера

            foreach (var pluginId in plugins)
            {

                PluginResponse response;

                try
                {
                    var responseResult = executor.ExecutePlugin(request, pluginId); // Здесь происходит ассинхронное выполнение запроса
                    response = responseResult.Response;
                }
                catch (Exception e)
                {
                    resultBuilder.AddMissedResponse(e);
                    continue;
                }

                resultBuilder.AddPluginResponse(response);

            }

            // Возвращение конечного ответа от сервера происходит при помощи метода GetResult() экземпляра класса RequestResultBuilder.
            // Он, основываясь на переданных ему ответах от плагинов, создает экземпляр класса ActionResult, который будет передан расширению клиента,
            // где будет обработан и отображен в браузере пользователя.
            return resultBuilder.GetResult();
        }

        public ActionResult Index(string RequestString, string UserToken)
        {
            var httpRequest = Request;
            var reader = new StreamReader(httpRequest.InputStream);
            var headerText = reader.ReadToEnd();
            var request = PluginRequestModel.CreateDefault();
            var statusProvider = new PluginsPhysicalStatusProvider(Repository, Collection);

            Transactor.Sync(statusProvider);
                
            var pluginRequest = request.CreatePluginRequest(); // Запрос, отсылаемый к плагину
            var executor = new PluginsExecutor(Transactor, Repository, Collection); // Предоставляет методы для получения ответа от плагина
            var plugins = GetPluginsForRequest(request); // Коллекция идентификаторов плагинов, от которых следует получить ответ 
            var resultBuilder = new RequestResultBuilder(request); // Предоставляет методы для получния конечного ответа от сервера

            foreach (var pluginId in plugins)
            {

                PluginResponse response;

                try
                {
                    var responseResult = executor.ExecutePlugin(request, pluginId); // Здесь происходит ассинхронное выполнение запроса
                    response = responseResult.Response;
                }
                catch (Exception e)
                {
                    resultBuilder.AddMissedResponse(e);
                    continue;
                }

                resultBuilder.AddPluginResponse(response);

            }

            // Возвращение конечного ответа от сервера происходит при помощи метода GetResult() экземпляра класса RequestResultBuilder.
            // Он, основываясь на переданных ему ответах от плагинов, создает экземпляр класса ActionResult, который будет передан расширению клиента,
            // где будет обработан и отображен в браузере пользователя.
            return resultBuilder.GetResult();
        }
	}
}