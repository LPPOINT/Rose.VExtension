namespace Rose.VExtension.PluginSystem.Console
{
    public interface IConsolableDomain
    {
        void ExecuteConsoleCommand(ConsoleCommand command, ConsoleResponse response);
    }
}
