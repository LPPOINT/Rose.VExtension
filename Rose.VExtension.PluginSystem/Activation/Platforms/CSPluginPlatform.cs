using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ninject;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Activation.Platforms
{
    /// <summary>
    /// Платформа плагина на языке CSharp
    /// </summary>
    [Platform("CSharp")]
    public class CSPluginPlatform : IPluginPlatform
    {
        public CSPluginPlatform(Plugin plugin)
        {
            Plugin = plugin;
            var config = plugin.PluginConfiguration;
            var kernel = new StandardKernel(new InitializationModule());
            var syntax = kernel.Get<IConfigurationSyntax>();

            var platformSection = config.GetItem(syntax.PlatformItem);

            var platformName = platformSection.GetContentValue("Name");


            var assemblies = platformSection.GetFirstChild().WrapTo<AssembliesConfigItemWrapper>();
            Assemblies = new List<PluginAssembly>(assemblies);
            MainAssembly = Assemblies.First();
            PluginClassName = platformSection.GetContentValue("ControllerType");
            Type = PlatformType.CSharp;;

        }

        public Plugin Plugin { get; private set; }

        public PlatformType Type { get; private set; }


        private Type GetTypeFromAssemblies(IEnumerable<Assembly> assemblies, string type)
        {
            return assemblies.Select(assembly => assembly.GetType(type)).FirstOrDefault(maybeType => maybeType != null);
        }
        public void SaveStreamToFile(string filename, Stream stream)
        {
            if (stream.Length != 0)
                using (var fileStream = File.Create(filename, (int)stream.Length))
                {
                    var data = new byte[stream.Length];

                    stream.Read(data, 0, data.Length);
                    fileStream.Write(data, 0, data.Length);
                }
        }


        public void BuildDomain()
        {
            var fileSystem = Plugin.FileSystem;
            var assemblies = new List<Assembly>();

            var platformAssembliesDef = Assemblies;
            var directory = Plugin.Id;

            Directory.CreateDirectory(directory);

            foreach (var pluginAssembly in platformAssembliesDef)
            {
                var path = Path.Combine(directory, pluginAssembly.AssemblyFileName);
                using (var assemblyStream = fileSystem.GetItemStream(FileSystemItem.GetAssemblyItem(pluginAssembly.AssemblyFileName)))
                {
                    SaveStreamToFile(path, assemblyStream);
                }
            }


            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                var fullPath = Path.GetFullPath(file);
                var name = AssemblyName.GetAssemblyName(fullPath);
                var assembly = Assembly.Load(name);
                assemblies.Add(assembly);
            }


            var type = GetTypeFromAssemblies(assemblies, PluginClassName);

            if (type == null)
                throw new PluginControllerInitializationException("Не найден контролеер плагина в заданных сборках");

            try
            {
                var domain = Activator.CreateInstance(type) as PluginDomain;
                domain.Plugin = Plugin;
                Plugin.Domain = domain;

            }
            catch (Exception e)
            {
                throw new PluginControllerInitializationException("Невозможно создать экземпляр контроллера плагина. Подробнее в InnerException", e);
            }

        }

        /// <summary>
        /// Список сборок, которые нужны для функционирования плагина
        /// </summary>
        public IEnumerable<PluginAssembly> Assemblies { get; private set; }

        /// <summary>
        /// Сборка, в которой находится контроллер плагина
        /// </summary>
        public PluginAssembly MainAssembly { get; private set; }
        /// <summary>
        /// Имя контроллера плагина
        /// </summary>
        public string PluginClassName { get; private set; }

    }
}
