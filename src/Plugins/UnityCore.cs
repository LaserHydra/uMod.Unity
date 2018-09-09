using uMod.Plugins;
using uMod.Unity.Logging;

namespace uMod.Unity.Plugins
{
    /// <summary>
    /// The core Unity plugin
    /// </summary>
    public class UnityCore : CSPlugin
    {
        private UnityLogger logger;

        /// <summary>
        /// Initializes a new instance of the UnityCore class
        /// </summary>
        public UnityCore()
        {
            // Set plugin info attributes
            Title = "Unity";
            Author = UnityExtension.AssemblyAuthors;
            Version = UnityExtension.AssemblyVersion;
        }

        /// <summary>
        /// Called when the it's safe to initialize logging
        /// </summary>
        [HookMethod("InitLogging")]
        private void InitLogging()
        {
            // Create our logger and add it to the compound logger
            Interface.uMod.NextTick(() =>
            {
                logger = new UnityLogger();
                Interface.uMod.RootLogger.AddLogger(logger);
                Interface.uMod.RootLogger.DisableCache();
            });
        }
    }
}
