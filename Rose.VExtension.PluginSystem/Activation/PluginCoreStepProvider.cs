using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NLog;
using Rose.VExtension.PluginSystem.Activation.RuntimeActivation;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Permissions;
using Rose.VExtension.PluginSystem.Reservation;
using Rose.VExtension.PluginSystem.Storage;
using Rose.VExtension.PluginSystem.UserSettings;
using Rose.VExtension.PluginSystem.Validation;

namespace Rose.VExtension.PluginSystem.Activation
{
    public class PluginCoreStepProvider
    {

        [ActivationStep(ActivationStepName.VersionActivation)]
        public void InitializePluginVersion(Plugin plugin, ActivationInfo info)
        {
            try
            {
                var validator = info.Validator;
                var syntax = info.Syntax;
                var config = plugin.PluginConfiguration;
                var versionString = config.GetItemValue(syntax.VersionPath);
                validator.ValidatePluginVersion(versionString);
                Version version;
                var versionParseResult = Version.TryParse(versionString, out version);
                if (!versionParseResult)
                    throw new Exception("Строка версии имеет неверный формат");
                plugin.Version = version;
            }
            catch (Exception e)
            {
                plugin.Version = new Version(1, 1, 1, 1);
                throw new ActivationStepException("Не удалось разобрать версию плагина", e, ActivationStepName.VersionActivation);
            }
        }

        [ActivationStep(ActivationStepName.MetaActivation)]
        public void InitializePluginMeta(Plugin plugin, ActivationInfo info)
        {
            try
            {
                var syntax = info.Syntax;
                var config = plugin.PluginConfiguration;
                var meta = config.GetItem(syntax.MetaPath).WrapTo<MetaItemWrapper>();
                plugin.Meta = meta.Meta;
            }
            catch (Exception e)
            {
                throw new ActivationStepException("Не удалось разобрать мета-информации о плагине", e, ActivationStepName.MetaActivation);
            }
        }

        [ActivationStep(ActivationStepName.PermissionsActivation)]
        public void InitializePluginPermissions(Plugin plugin, ActivationInfo info)
        {
            var config = plugin.PluginConfiguration;
            const string permissionsSectionName = "Permissions";
            const string permissionSectionName = "Permission";

            plugin.Permissions = new PluginPersmissionCollection();

            IConfigurationItem permissionsSection;

            try
            {
                permissionsSection = config.GetItem("Manifest/" + permissionsSectionName + "/");
            }
            catch 
            {
                return;
            }

            if(permissionsSection == null)
                return;

            var multyException =
                new ActivationMultiException("Невозможно разобрать одно или несколько разрешений плагина",
                    ActivationStepName.PermissionsActivation);

            foreach (var perContent in permissionsSection.Content.Where(pair => pair.Key == permissionSectionName))
            {
                try
                {
                    plugin.Permissions.Add(new PluginPermission(perContent.Value));
                }
                catch (Exception e)
                {
                    multyException.InnerActivationExceptions.Add(e);
                }
            }
            foreach (var innerPer in permissionsSection.InnerItems.Where(item => item.Name == permissionSectionName))
            {
                try
                {
                    plugin.Permissions.Add(innerPer.WrapTo<PluginPermission>());
                }
                catch (Exception e)
                {
                    multyException.InnerActivationExceptions.Add(e);
                }
            }

            if (multyException.InnerActivationExceptions.Any())
                throw multyException;

        }

        [ActivationStep(ActivationStepName.StorageActivation)]
        public void InitializePluginStorage(Plugin plugin, ActivationInfo info)
        {
            try
            {
                plugin.Storage = new PluginStorage();
            }
            catch (Exception e)
            {
                throw new ActivationStepException("Не удалось инициализировать хранилище плагина", e, ActivationStepName.StorageActivation, true);
            }
        }

        [ActivationStep(ActivationStepName.ReservationActivation)]
        public void InitializePluginReservation(Plugin plugin, ActivationInfo info)
        {
            try
            {
                var reservator = new PluginReservator(info.ReservationRepository);

                if (reservator.IsReservedPlugin(plugin))
                {
                    var id = reservator.GetPluginId(plugin);
                    plugin.Id = id;
                }
                else
                {
                    var id = reservator.ReservatePlugin(plugin);
                    plugin.Id = id;
                }
            }
            catch (Exception e)
            {
                throw new ActivationStepException("Не удалось определить резервацию плагина", e, ActivationStepName.ReservationActivation);
            }
        }

        [ActivationStep(ActivationStepName.NameActivation)]
        public void InitializePluginName(Plugin plugin, ActivationInfo info)
        {
            try
            {
                var validator = new PluginValidator();
                var config = plugin.PluginConfiguration;
                var name = config.RootItem.Content.FirstOrDefault(pair => pair.Key == "Name").Value;
                validator.ValidatePluginName(name);
                plugin.Name = name;
            }
            catch (ConfigItemNotFoundException e)
            {
                throw new ActivationStepException("В файле конфигурации не определено имя плагина", e, ActivationStepName.NameActivation, true);
            }
            catch (Exception e)
            {
                throw new PluginInitializationException("Не удалось определить имя плагина", e);
            }
        }

        [ActivationStep(ActivationStepName.SettingsActivation)]
        public void InitializePluginSettings(Plugin plugin, ActivationInfo info)
        {
            IPluginConfiguration config = plugin.PluginConfiguration;
            var settingsNode = config.RootItem.Content.FirstOrDefault(pair => pair.Key == "Settings").Value;

            if (!string.IsNullOrWhiteSpace(settingsNode))
            {
                using (
                    var settingsFileStream =
                        plugin.FileSystem.GetItemStream(FileSystemItem.GetSettingsFileItem(settingsNode)))
                {
                    try
                    {
                        var xml = XDocument.Load(settingsFileStream);
                        var source = new XDocumentSettingsSource(xml);
                        var settings = source.GetSettings();
                        plugin.Settings = settings;
                    }
                    catch (Exception e)
                    {
                        throw new ActivationStepException("Не удалось загрузить файл пользовательских настроек плагина", e, ActivationStepName.SettingsActivation);
                    }
                }
            }
        }
    }
}
