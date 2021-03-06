﻿using Rose.VExtension.Server.DbInteraction;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Transactions;

namespace Rose.VExtension.Server.Models
{
    public class PluginsControllerContext
    {
        public PluginsControllerContext(IPluginsRepository repository, IPluginsCollection plugins, IPluginTransactor transactor)
        {
            Transactor = transactor;
            Plugins = plugins;
            Repository = repository;
        }

        public IPluginsRepository Repository { get; private set; }
        public IPluginsCollection Plugins { get; private set; }
        public IPluginTransactor Transactor { get; private set; }
    }
}