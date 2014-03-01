using System;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;
using NLog;
using Noesis.Javascript;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Packing;
using Rose.VExtension.PluginSystem.Reservation;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystemConsole
{
    class Program
    {

        public static void PluginTets()
        {
            var stopwatch = new Stopwatch();
            var packge =
                new LocalStoragePluginPackage(
                    @"C:\Users\Sasha\Documents\Visual Studio 2013\Projects\Rose.VExtension\plugin.zip",
                    PacckageType.ZipFile);
            var fileSystem = new LocalPluginFileSystem(@"C:\Users\Sasha\Documents\Visual Studio 2013\Projects\Rose.VExtension\Content\TestFs");
            var factory = new PluginFactory(new PluginInitializer(new PluginCollectionReservationRepository()));

            stopwatch.Start();

            var handler = new LogInitializationHandler(LogManager.GetCurrentClassLogger());
            var config = factory.ToFileSystem(fileSystem, packge);

            stopwatch.Restart();

            var plugin = factory.ToRunnable(fileSystem, config, packge, handler);
            

            Console.ReadKey();
        }

        public static void JavascriptTest()
        {
        }

        static void Main(string[] args)
        {
            PluginTets();

        }
    }
}
