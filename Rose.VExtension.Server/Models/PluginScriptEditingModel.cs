namespace Rose.VExtension.Server.Models
{
    public class PluginScriptEditingModel
    {
        public PluginScriptEditingModel()
        {
            
        }

        public PluginScriptEditingModel(string pluginId, string scriptUri, string script)
        {
            PluginId = pluginId;
            ScriptUri = scriptUri;
            Script = script;
        }

        public string PluginId { get; set; }
        public string ScriptUri { get; private set; }
        public string Script { get; private set; }
    }
}