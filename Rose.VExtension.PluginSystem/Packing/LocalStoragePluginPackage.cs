﻿using System;
using System.IO;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.Common;

namespace Rose.VExtension.PluginSystem.Packing
{
    public class LocalStoragePluginPackage : IPluginPackage
    {
        public LocalStoragePluginPackage(string uri, PacckageType type)
        {
            Check.NotNullOrWhiteSpace(uri);
            Check.NotNull(type);

            Type = type;
            URI = uri;
        }

        public string URI { get; private set; }
        public PacckageType Type { get; private set; }

        public void FromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream GetStream()
        {
            return new FileStream(URI, FileMode.OpenOrCreate);
        }
    }
}
