namespace Rose.VExtension.PluginSystem.Console
{
    public interface IConsoleCommandParser
    {
        ConsoleCommand Parse(string str);
    }
}
