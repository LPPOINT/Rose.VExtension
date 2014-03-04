using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.Models
{

    /// <summary>
    /// Обеспечивает соединение между плагиноми его представлением на сервере
    /// </summary>
    public interface IPluginConnection
    {

        /// <summary>
        /// Плагин
        /// </summary>
        PluginSystem.Plugin ControlPlugin { get; }

        /// <summary>
        /// Представление плагина на сервере
        /// </summary>
        Plugin PluginEntity { get; }

        /// <summary>
        /// Репозиторий плагинов на сервере
        /// </summary>
        IPluginsRepository Repository { get; }

        /// <summary>
        /// При переопределении начинает взаимодействие между плагином и его представлением на сервере
        /// </summary>
        void Open();

        /// <summary>
        /// Идентификатор соединения, равный идентификатору плагина
        /// </summary>
        string ConnectionId { get; }

        /// <summary>
        /// При переопределении обрывает взаимодействие между плагином и его представлением на сервере
        /// </summary>
        void Close();

        /// <summary>
        /// При переопределении удаляет все сущности плагина на сервере и закрывает соединение
        /// </summary>
        void Drop();

        void Drop(ConnectionDropOptions options);

        /// <summary>
        /// Синхронизиреут плагин с его представлением на сервере
        /// </summary>
        void Syncronize();

    }
}
