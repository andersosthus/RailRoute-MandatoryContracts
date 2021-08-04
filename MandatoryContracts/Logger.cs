using System;
using System.Reflection;

namespace MandatoryContracts
{
    public static class Logger
    {
        static string AssemblyName = Assembly.GetExecutingAssembly().FullName.Split(',')[0];

        public static void Debug(string msg)
        {
#if DEBUG
            UnityEngine.Debug.Log($"{AssemblyName} - Debug: {msg}");
#endif
        }

        public static void Info(string msg)
        {
            UnityEngine.Debug.Log($"{AssemblyName} - Info: {msg}");
        }

        public static void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning($"{AssemblyName} - Error: {msg}");
        }

        public static void Error(string msg)
        {
            UnityEngine.Debug.LogError($"{AssemblyName} - Error: {msg}");
        }

        public static void Ex(Exception ex)
        {
            UnityEngine.Debug.LogError($"{AssemblyName} - Exception:");
            UnityEngine.Debug.LogException(ex);
        }
	}
}
