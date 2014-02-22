using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Xml.Serialization;
using NLog;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.Common;

namespace Rose.VExtension.PluginSystem.Reservation
{
    public static class PluginReservationManager
    {


        private static readonly IPluginsReservationRepository Repository = new PluginCollectionReservationRepository(DeserializeDictionary());

        public static void SaveRepositoryDictionary(Dictionary<string, string> dictionary)
        {
            try
            {
                var dic = new SerializableDictionary<string, string>();

                foreach (var item in dictionary)
                {
                    dic.Add(item.Key, item.Value);
                }

                var path = @"C:\Users\Sasha\Documents\visual studio 2013\Projects\Rose.VExtension\Rose.VExtension.PluginSystem\test.xml";
                var serializaer = new XmlSerializer(typeof (SerializableDictionary<string, string>));
                using (var file = new FileStream(path, FileMode.OpenOrCreate))
                {
                    serializaer.Serialize(file, dictionary);
                }
            }
            catch (Exception e)
            {
               LogManager.GetCurrentClassLogger().WarnException("Не удалось сохранить коллекцию", e);
            }
        }

        public static void SerializeDictionary()
        {
            SerializeDictionary((Repository as PluginCollectionReservationRepository).collection);
        }

        public static void SerializeDictionary(Dictionary<string, string> dictionary)
        {
            try
            {
                using (var streamWriter = new StreamWriter("MyDictionary.xml"))
                {

                    var serializable = new SerializableDictionary<string, string>();

                    foreach (var item in dictionary)
                    {
                        serializable.Add(item.Key, item.Value);
                    }

                    var xmlSerializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                    xmlSerializer.Serialize(streamWriter, serializable);
                }
            }
            catch (Exception e)
            {
               
            }
        }

        public static Dictionary<string, string> DeserializeDictionary()
        {
            try
            {
                SerializableDictionary<string, string> result;
                using (var streamReader = new StreamReader("MyDictionary.xml"))
                {
                    var xmlSerializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                    result = (SerializableDictionary<string, string>)(xmlSerializer.Deserialize(streamReader.BaseStream));
                }
                var d = new Dictionary<string, string>(result);
                return d;
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>();
            }
        }

        public static IPluginReservator GetReservator()
        {
            return new PluginReservator(Repository);
        }

        public static IPluginsReservationRepository GetReservationRepository()
        {
            return Repository;
        }

    }
}
