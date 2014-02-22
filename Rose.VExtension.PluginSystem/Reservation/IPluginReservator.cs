namespace Rose.VExtension.PluginSystem.Reservation
{

    /// <summary>
    /// Представляет методы для резервации плагина
    /// </summary>
    public interface IPluginReservator
    {

        IPluginsReservationRepository ReservationRepository { get; }

        /// <summary>
        /// Резервирует плагин, генерируя для него новый идентификатор
        /// </summary>
        /// <param name="plugin"></param>
        string ReservatePlugin(Plugin plugin);

        /// <summary>
        /// Резервирует плагин по заданному идентификатору
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="id"></param>
        string ReservatePlugin(Plugin plugin, string id);

        bool IsReservedPlugin(Plugin plugin);

        string GetPluginId(Plugin plugin);

        /// <summary>
        /// Возращает сид, с помощью которого плагин будет зарезервирован
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        string GetReservationSeed(Plugin plugin);

    }

    public class PluginReservator : IPluginReservator
    {
        public PluginReservator(IPluginsReservationRepository reservationRepository)
        {
            ReservationRepository = reservationRepository;
        }

        public IPluginsReservationRepository ReservationRepository { get; private set; }

        public string ReservatePlugin(Plugin plugin)
        {
            var id = ReservationRepository.GenerateNewId();
            return ReservatePlugin(plugin, id);

        }

        public string ReservatePlugin(Plugin plugin, string id)
        {
            var seed = GetReservationSeed(plugin);

            if (seed != null)
            {
                ReservationRepository.AssociatePluginString(seed, id);
            }

            return id;

        }

        public bool IsReservedPlugin(Plugin plugin)
        {
            var res = ReservationRepository.GetIdForStringAssociation(GetReservationSeed(plugin));
            return
                !string.IsNullOrWhiteSpace(res);
        }

        public string GetPluginId(Plugin plugin)
        {
            return ReservationRepository.GetIdForStringAssociation(plugin.Name);
        }

        /// <summary>
        /// Сидирование по имени (Одинаковые имена не допускаются)
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        public string GetReservationSeed(Plugin plugin)
        {
            return plugin.Name;
        }
    }

}
