using System;
using System.Globalization;
using System.IO;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.Packing;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.Middleware
{
    public class PackageMiddleware : IPluginComponentMiddleware<IPluginPackage, PluginPackage>
    {
        public IPluginPackage CreateBase(PluginPackage entity)
        {
            if (entity is Models.DbInteraction.LocalStoragePluginPackage)
            {
                var ls = entity as Models.DbInteraction.LocalStoragePluginPackage;
                return new PluginSystem.Packing.LocalStoragePluginPackage(ls.RootPath, PacckageType.ZipFile);
            }
            if (entity is StreamPackage)
            {
                var sp = entity as StreamPackage;
                var stream = GetStream(sp.StreamFileUri);
                return new StreamPluginPackage(stream);
            }
            throw new NotSupportedException();
        }

        private static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        private void SaveStream(Stream stream, string path)
        {

            if (!Directory.Exists(path))
            {
                var directory = Path.GetDirectoryName(path);
                Directory.CreateDirectory(directory);
            }

            using (Stream file = File.Create(path))
            {
                CopyStream(stream, file);
            }
        }

        private Stream GetStream(string path)
        {
            try
            {
                return new FileStream(path, FileMode.Open);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string GetStreamFileName(StreamPluginPackage package)
        {
            var hash = package.GetHashCode().ToString(CultureInfo.InvariantCulture);
            return Path.Combine(@"C:\Users\Sasha\Documents\Visual Studio 2013\Projects\Rose.VExtension\Content", "TempPackageMiddlewareStreams", hash);
        }

        public PluginPackage CreateEntity(IPluginPackage @base)
        {
            if (@base is PluginSystem.Packing.LocalStoragePluginPackage)
            {
                var ls = @base as PluginSystem.Packing.LocalStoragePluginPackage;
                return new Models.DbInteraction.LocalStoragePluginPackage
                       {
                           RootPath = ls.URI
                       };
            }
            if (@base is PluginSystem.Packing.StreamPluginPackage)
            {
                var ps = @base as PluginSystem.Packing.StreamPluginPackage;
                var path = GetStreamFileName(ps);
                SaveStream(ps.Stream, path);

                return new StreamPackage
                       {
                           StreamFileUri = path
                       };

            }

            throw new NotSupportedException();
        }
    }
}
