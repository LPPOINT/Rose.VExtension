using System;
using System.Collections.Generic;
using System.Linq;

namespace Rose.VExtension.PluginSystem.Resources
{
    public class PluginResourcesTokenGenerator : IPluginResourceTokenGenerator
    {
        public string GenerateToken(IEnumerable<string> alreadyUsedTokens)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
