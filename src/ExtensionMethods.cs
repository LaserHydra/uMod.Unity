using System;
using System.Linq;
using UnityEngine;
using CoreLogging = uMod.Logging;

namespace uMod.Unity
{
    /// <summary>
    /// Useful extension methods which are added to base types
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts a comma delimited string to a UnityEngine Vector3
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this string vector3)
        {
            float[] split = vector3.Split(',').Select(Convert.ToSingle).ToArray();
            return split.Length == 3 ? new Vector3(split[0], split[1], split[2]) : Vector3.zero;
        }

        /// <summary>
        /// Converts a UnityEngine LogType to the universal LogType format
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        public static CoreLogging.LogType ToLogType(this LogType logType)
        {
            switch (logType)
            {
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                    return CoreLogging.LogType.Error;

                case LogType.Warning:
                    return CoreLogging.LogType.Warning;

                case LogType.Log:
                    return CoreLogging.LogType.Info;

                default:
                    return CoreLogging.LogType.Info;
            }
        }
    }
}
