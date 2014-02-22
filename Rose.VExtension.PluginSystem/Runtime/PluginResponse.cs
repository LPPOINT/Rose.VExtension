using HtmlAgilityPack;

namespace Rose.VExtension.PluginSystem.Runtime
{
    public class PluginResponse
    {
        public int ResponseCode { get; set; }
        public HtmlDocument Html { get; set; }

        public void InjectScript(string script)
        {
            
        }

        public void InjectScriptFile(string fileName, Plugin plugin)
        {
            
        }

    }
}
