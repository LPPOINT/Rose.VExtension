using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rose.VExtension.PluginSystem.Common;

namespace Rose.VExtension.PluginSystem.Runtime.RequestHandeling
{

    /// <summary>
    /// Содержит методы для управления обработчиками запросов
    /// </summary>
    public interface IPluginRequestController : IList<IPluginRequestHandler>
    {
        /// <summary>
        /// При переопределении Возвращает все обработчики, фильтры которых завалидировали заданный запрос
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<IPluginRequestHandler> GetHandlersForRequest(PluginRequest request);

        IPluginActivity GetActivityByName(string name);

    }

    public class PluginRequestController : IPluginRequestController
    {

        public PluginRequestController()
        {
            Handlers = new List<IPluginRequestHandler>();
        }

        public List<IPluginRequestHandler> Handlers { get; private set; } 

        public IEnumerator<IPluginRequestHandler> GetEnumerator()
        {
            return Handlers.GetEnumerator();
        }

        public IEnumerable<IPluginRequestHandler> GetHandlersForRequest(PluginRequest request)
        {
            return Handlers.Where(handler => handler.Filter.IsValidRequest(request));
        }

        public IPluginActivity GetActivityByName(string name)
        {
            Check.NotNullOrWhiteSpace(name);
            return Handlers.FirstOrDefault(handler => handler.Activity.Name == name).Activity;

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IPluginRequestHandler item)
        {
            Handlers.Add(item);
        }

        public void Clear()
        {
            Handlers.Clear();;
        }

        public bool Contains(IPluginRequestHandler item)
        {
            return Handlers.Contains(item);
        }

        public void CopyTo(IPluginRequestHandler[] array, int arrayIndex)
        {
            Handlers.CopyTo(array, arrayIndex);
        }

        public bool Remove(IPluginRequestHandler item)
        {
            return Handlers.Remove(item);
        }

        public int Count { get { return Handlers.Count; }}
        public bool IsReadOnly { get { return false; } }
        public int IndexOf(IPluginRequestHandler item)
        {
            return Handlers.IndexOf(item);
        }

        public void Insert(int index, IPluginRequestHandler item)
        {
            Handlers.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Handlers.RemoveAt(index);
        }

        public IPluginRequestHandler this[int index]
        {
            get { return Handlers[index]; }
            set { Handlers[index] = value; }
        }
    }

}
