using System;
using uMod.Plugins;

namespace uMod.Unity.Plugins
{
    /// <summary>
    /// Responsible for loading Unity core plugins
    /// </summary>
    public class UnityPluginLoader : PluginLoader
    {
        public override Type[] CorePlugins => new[] { typeof(UnityCore) };
    }
}
