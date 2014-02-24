using Rose.VExtension.PluginSystem.Common;

namespace Rose.VExtension.PluginSystem.Runtime.RequestHandeling
{

    /// <summary>
    /// Представляет обработчик для заданных запросов
    /// </summary>
    public interface IPluginRequestHandler
    {
        /// <summary>
        /// Фильтр запросов
        /// </summary>
        IPluginRequestFilter Filter { get; }

        /// <summary>
        /// Activity, выполняемое для заданного фильтра
        /// </summary>
        IPluginActivity Activity { get; }
    }

    public class PluginRequestHandler : IPluginRequestHandler
    {
        public PluginRequestHandler(IPluginRequestFilter filter, IPluginActivity activity)
        {

            Check.NotNull(filter);
            Check.NotNull(activity);

            Activity = activity;
            Filter = filter;
        }

        public IPluginRequestFilter Filter { get; private set; }
        public IPluginActivity Activity { get; private set; }
    }

}
