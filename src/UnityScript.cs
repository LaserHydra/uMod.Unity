using System;
using System.Reflection;
using uMod.Logging;
using UnityEngine;
using LogType = UnityEngine.LogType;

#pragma warning disable 0618

namespace uMod.Unity
{
    /// <summary>
    /// The main MonoBehaviour which calls uMod.OnFrame
    /// </summary>
    public class UnityScript : MonoBehaviour
    {
        public static GameObject Instance { get; private set; }

        public static void Create()
        {
            Instance = new GameObject("uMod.Unity");
            DontDestroyOnLoad(Instance);
            Instance.AddComponent<UnityScript>();
        }

        private uMod uMod;

        private void Awake()
        {
            EventInfo eventInfo = typeof(Application).GetEvent("logMessageReceived");
            uMod = Interface.uMod;

            if (eventInfo == null)
            {
                // Unity 4
                FieldInfo logCallbackField = typeof(Application).GetField("s_LogCallback", BindingFlags.Static | BindingFlags.NonPublic);
                Application.LogCallback logCallback = logCallbackField?.GetValue(null) as Application.LogCallback;

                if (logCallback == null)
                {
                    Interface.Oxide.LogWarning("No Unity application log callback is registered");
                }

                Application.RegisterLogCallback((message, stackTrace, type) =>
                {
                    logCallback?.Invoke(message, stackTrace, type);
                    LogMessageReceived(message, stackTrace, type);
                });
            }
            else
            {
                // Unity 5
                Delegate handleException = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, "LogMessageReceived");
                eventInfo.GetAddMethod().Invoke(null, new object[] { handleException });
            }
        }

        private void Update() => uMod.OnFrame(Time.deltaTime);

        private void OnDestroy()
        {
            if (!uMod.IsShuttingDown)
            {
                uMod.LogWarning("The uMod Unity script was destroyed (creating a new instance)");
                uMod.NextTick(Create);
            }
        }

        private void OnApplicationQuit()
        {
            if (!uMod.IsShuttingDown)
            {
                Interface.Call("OnServerShutdown");
                Interface.Oxide.OnShutdown();
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        // This CANNOT be static! The world may impode if so, maybe worse
        private void LogMessageReceived(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                RemoteLogger.Exception(message, stackTrace);
            }
        }
    }
}
