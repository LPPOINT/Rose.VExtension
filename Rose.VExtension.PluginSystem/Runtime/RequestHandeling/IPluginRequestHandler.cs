using Rose.VExtension.PluginSystem.Common;
using Rose.VExtension.PluginSystem.Permissions;

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
        /// Аргументы, который требуется передать деятельности при выполнении
        /// </summary>
        RequestArgumentsCollection Arguments { get; }

        /// <summary>
        /// Метод воздействия деятельности на страницу
        /// </summary>
        PageEditingMethod Method { get; }

        /// <summary>
        /// Activity, выполняемое для заданного фильтра
        /// </summary>
        IPluginActivity Activity { get; }
    }

    public class PluginRequestHandler : IPluginRequestHandler
    {
        public PluginRequestHandler(IPluginRequestFilter filter, IPluginActivity activity, RequestArgumentsCollection arguments)
        {
            Check.NotNull(filter);
            Check.NotNull(activity);
            Check.NotNull(arguments);

            Arguments = arguments;
            Activity = activity;
            Filter = filter;
        }

        public IPluginRequestFilter Filter { get; private set; }
        public RequestArgumentsCollection Arguments { get; private set; }
        public PageEditingMethod Method { get; private set; }
        public IPluginActivity Activity { get; private set; }
    }

}
