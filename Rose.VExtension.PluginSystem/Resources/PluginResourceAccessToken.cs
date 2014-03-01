using System;
using System.Collections.Generic;
using Ninject;

namespace Rose.VExtension.PluginSystem.Resources
{
    public class PluginResourceAccessToken
    {
        public PluginResourceAccessToken(string token, TimeSpan duration, DateTime validBefore, string resourcePath)
        {
            ResourcePath = resourcePath;
            if (duration != TimeSpan.Zero || validBefore > DateTime.Now)
            {
                ValidBefore = validBefore;
                Duration = duration;
            }
            else
            {
                Duration = TimeSpan.Zero;
                ValidBefore = DateTime.MinValue;
            }
            Token = token;
        }

        public PluginResourceAccessToken(string token, TimeSpan duration, string resourceName) 
            : this(token, duration, DateTime.Now + duration, resourceName)
        {
        }


        public static PluginResourceAccessToken Create(string resName, TimeSpan duration, IPluginResourceTokenGenerator tokenGenerator)
        {
            var token = tokenGenerator.GenerateToken(new List<string>());
            return new PluginResourceAccessToken(token, duration, resName);
        }
        public static PluginResourceAccessToken Create(string resName, TimeSpan duration)
        {
            var kernel = new StandardKernel(new PluginDataNinjectModule());
            var generator = kernel.Get<IPluginResourceTokenGenerator>();

            if (generator != null)
            {
                return Create(resName, duration, generator);
            }

            return null;
        }
        public static PluginResourceAccessToken Create(string resName)
        {
            return Create(resName, new TimeSpan(9999, 0, 0, 0));
        }

        public string Token { get;  set; }
        public TimeSpan Duration { get;  set; }
        public DateTime ValidBefore { get;  set; }
        public string ResourcePath { get;  set; }

        public string Url { get; set; }

        public bool IsInfinity
        {
            get { return Duration == TimeSpan.Zero; }
        }

    }
}
