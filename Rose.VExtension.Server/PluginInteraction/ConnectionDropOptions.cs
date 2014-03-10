namespace Rose.VExtension.Server.Models
{

    /// <summary>
    /// Опции сброса соединения с плагином
    /// </summary>
    public class ConnectionDropOptions
    {
        /// <summary>
        /// Удалить папку плагина
        /// </summary>
        public bool DeleteFileSystem { get; set; }
        /// <summary>
        /// Удалить пакет плагина
        /// </summary>
        public bool DeletePackage { get; set; }
        /// <summary>
        /// Удалить резервацию плагина
        /// </summary>
        public bool DeleteReservation { get; set; }
        /// <summary>
        /// Удалить элементы в хранилище плагина
        /// </summary>
        public bool DeleteStorageItems { get; set; }

        /// <summary>
        /// Опции сброса соединения по умолчанию
        /// </summary>
        public static ConnectionDropOptions Default = new ConnectionDropOptions {DeleteFileSystem = true, DeletePackage = false, DeleteReservation = true, DeleteStorageItems = true};
    }
}