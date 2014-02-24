using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Packing;
using Rose.VExtension.PluginSystem.Permissions;
using Rose.VExtension.PluginSystem.Resources;
using Rose.VExtension.PluginSystem.Runtime.RequestHandeling;
using Rose.VExtension.PluginSystem.Storage;
using Rose.VExtension.PluginSystem.UserSettings;

namespace Rose.VExtension.PluginSystem
{

    /// <summary>
    /// Представляет сведения о плагине и всех его сервисов. Основа всей системы плагинов
    /// </summary>
    public sealed class Plugin : IDisposable
    {


        public Plugin()
        {
            Logo = new PluginLogoProvider(this);
        }

        /// <summary>
        /// GUID плагина
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// Поставщик логотипа для плагина
        /// </summary>
        public IPluginLogoProvider Logo { get; private set; }

        /// <summary>
        /// Имя плагина
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Версия плагина
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Словарь мета-информации об плагине (Задается в манифесте плагина)
        /// </summary>
        public IDictionary<string, string> Meta { get; set; }

        /// <summary>
        /// Пакет плагина
        /// </summary>
        public IPluginPackage Package { get; set; }


        /// <summary>
        /// Контроллер, обрабатывающий входящий запрос
        /// </summary>
        public IPluginRequestController Controller { get; set; }

        /// <summary>
        /// Файловая система плагина. Предоставляет доступ к файлам в каталоге, в котором расположен плагин
        /// </summary>
        public IPluginFileSystem FileSystem { get; set; }

        /// <summary>
        /// Конфигурация плагина. Задается в файле Manifest.xml в корневом каталоге плагина
        /// </summary>
        public IPluginConfiguration PluginConfiguration { get; set; }

        /// <summary>
        /// Предоставляет доступ к ресурсам плагина (Изображение, аудио и т.д)
        /// </summary>
        public IPluginResourcesProvider ResourcesProvider { get; set; }

        /// <summary>
        /// Сведения о правах плагина
        /// </summary>
        public IPluginPermissions Permissions { get; set; }

        /// <summary>
        /// Настройки плагина, которые могут быть изменены клиентом. Значения настроек изменяются в зависимости от их значения у клиента
        /// </summary>
        public UserSettingsCollection Settings { get; set; }

        /// <summary>
        /// Хранилище пар значений, добавляемые во время выполнения плагина 
        /// </summary>
        public IPluginStorage Storage { get; set; }

        /// <summary>
        /// Возвращает строчное представление плагина
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
 
            builder.AppendFormat("ID: {0}\n", Id);
            builder.AppendFormat("Name: {0}\n", Name);
            builder.AppendFormat("FileSystem: {0}", FileSystem);

            return builder.ToString();

        }

        public bool Equals(Plugin other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Plugin) obj);
        }

        /// <summary>
        /// Возвращает хэш-код плагина, сформированный на основе его идентификатора
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        /// <summary>
        /// Создает плагин на основе его контекста
        /// </summary>
        /// <param name="context">Контекст плагина, на основе которого создается новый объект <see cref="Plugin"/></param>
        /// <param name="initializer">Объект, с помощью которого будет инициализироваться плагин после создания</param>
        /// <returns>Новый экземпляр класса <see cref="Plugin"/>, созданный на основе контекста</returns>
        public static Plugin Initialize(IPluginActivationContext context, IPluginInitializer initializer)
        {
            var factory = new PluginFactory(initializer);

            

            if (context is FileSystemPluginActivationContext)
            {
                var fsCtx = context as FileSystemPluginActivationContext;
                return factory.ToRunnable(fsCtx.FileSystem);
            }
             if (context is PackagePluginActivationContext)
             {
                 var pckCtx = context as PackagePluginActivationContext;
                 var config = factory.ToFileSystem(pckCtx.EmptyFileSystem, pckCtx.Package);
                 return factory.ToRunnable(pckCtx.EmptyFileSystem, config);
             }
            throw new ArgumentException("Неожиданный контекст плагина", "context");
        }

        /// <summary>
        /// Выгружает плагин, а также вызывает метод Dispose() всех доступных свойст плагина
        /// </summary>
        public void Dispose()
        {

            if (ResourcesProvider is IDisposable)
            {
                (ResourcesProvider as IDisposable).Dispose();
            }
            if (PluginConfiguration is IDisposable)
            {
                (PluginConfiguration as IDisposable).Dispose();
            }
            if (FileSystem is IDisposable)
            {
                (FileSystem as IDisposable).Dispose();
            }

        }
    }
}
