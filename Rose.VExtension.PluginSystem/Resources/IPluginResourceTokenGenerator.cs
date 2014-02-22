using System.Collections.Generic;

namespace Rose.VExtension.PluginSystem.Resources
{
    public interface IPluginResourceTokenGenerator
    {
        string GenerateToken(IEnumerable<string> alreadyUsedTokens);
    }
}
