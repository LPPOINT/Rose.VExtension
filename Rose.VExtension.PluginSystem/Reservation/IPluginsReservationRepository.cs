using System;
using System.Collections.Generic;
using System.Linq;

namespace Rose.VExtension.PluginSystem.Reservation
{

    /// <summary>
    /// Представляет набор методов для хранения ассоциированных с данными идентификаторов плагина 
    /// </summary>
    public interface IPluginsReservationRepository
    {


        void RemoveReservationById(string str);
        /// <summary>
        /// Ассоциирует строку и идентификатором плагина
        /// </summary>
        /// <param name="str"></param>
        /// <param name="id"></param>
        void AssociatePluginString(string str, string id);

        /// <summary>
        /// Ассоциирует число и идентификатором плагина
        /// </summary>
        /// <param name="i"></param>
        /// <param name="id"></param>

        /// <summary>
        /// При переопределении возращает ассоциированный с заданной строкой идентификатор плагина
        /// </summary>
        /// <returns></returns>
        string GetIdForStringAssociation(string str);

        /// <summary>
        /// При переопределении возращает значение, указывающее, является ли заданный идентификатор ассоциированным с какими-либо данными
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsAssociatedId(string id);

        /// <summary>
        /// При переопределении генерирует новый идентификатор плагина
        /// </summary>
        /// <returns></returns>
        string GenerateNewId();
    }

    public class PluginCollectionReservationRepository : IPluginsReservationRepository
    {


        public PluginCollectionReservationRepository()
        {
            collection = new Dictionary<string, string>();
        }
        public PluginCollectionReservationRepository(Dictionary<string, string> collection)
        {
            this.collection = collection;
        }

        public readonly Dictionary<string, string> collection;

        public void RemoveReservationById(string str)
        {
            collection.Remove(str);
        }

        public void AssociatePluginString(string str, string id)
        {
            collection.Add(str, id);
        }

        public string GetIdForStringAssociation(string str)
        {
            try
            {
                return collection[str];
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool IsAssociatedId(string id)
        {
            return collection.Values.Contains(id);
        }

        public string GenerateNewId()
        {
            return Guid.NewGuid().ToString();
        }
    }

}
