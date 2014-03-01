using System;
using System.Web.Mvc;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.Server.Models.Responsing
{

    /// <summary>
    /// Представляет методы для создания экземпляра класса <see cref="ActionResult"/> используя ответы от плагина
    /// </summary>
    public interface IRequestResutBuilder
    {

        /// <summary>
        /// Модель запроса, переданного от елиентского расширения к серверу
        /// </summary>
        PluginRequestModel PluginRequest { get; }

        /// <summary>
        /// Добавляет ответ от плагина
        /// </summary>
        /// <param name="response"></param>
        void AddPluginResponse(PluginResponse response);

        /// <summary>
        /// Обозначает ответ от плагина, который не может быть добавлен из-за заданного исключения
        /// </summary>
        /// <param name="missException">Исключение, объясняющие причину игнорирования ответа от плагина</param>
        void AddMissedResponse(Exception missException);

        /// <summary>
        /// Создает экземпляр класса <see cref="ActionResult"/>
        /// </summary>
        /// <returns></returns>
        ActionResult GetResult();
    }
}
