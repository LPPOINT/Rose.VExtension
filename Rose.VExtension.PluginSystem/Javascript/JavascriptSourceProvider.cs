using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.Activation.Platforms;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Javascript
{
    /// <summary>
    /// ������������ ������ ��� ��������� � ��������� �������� ����� Javascript-������
    /// </summary>
    public class JavascriptSourceProvider
    {
        public string GetSources(Plugin plugin, JSPluginPlatform jsPluginPlatform)
        {

           if(jsPluginPlatform.Scripts.Count != 1)
               throw new NotImplementedException("���������� ���� �� ���������� ������ �� ������������");

            var file = jsPluginPlatform.Scripts.First();

            using (var stream = plugin.FileSystem.GetItemStream(FileSystemItem.GetScriptItem(file)))
            {
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    var script = reader.ReadToEnd();
                    return script;
                }

                // typeof(string).GetMethods(BindingFlags.Public)

            }

        }
    }
}