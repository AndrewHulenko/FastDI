using UnityEngine;

namespace FastDI
{
    public static class BinderUtils
    {
        public const int ErrorAlreadyRegistered = 10;
        public const int ErrorNotRegistered = 11;
        public const int ErrorCantCreateContainer = 20;
        public const int ErrorCantGetContainer = 21;
        public const int WarningFoundMoreThenOne = 30;
        
        public static void PrintWarning(int code, string message, object instance = null)
        {
            MonoBehaviour monoBehaviour = instance as MonoBehaviour;
            GameObject gameObject = monoBehaviour != null ? monoBehaviour.gameObject : null;
            Debug.Log("<color=#e6ac2f><b>[ FastDI, code: " + code + "] " + message + "</b></color>", gameObject);
        }

        public static void PrintError(int code, string message, object instance = null)
        {
            MonoBehaviour monoBehaviour = instance as MonoBehaviour;
            GameObject gameObject = monoBehaviour != null ? monoBehaviour.gameObject : null;
            Debug.Log("<color=#e15656><b>[ FastDI, code: " + code + "] " + message + "</b></color>", gameObject);
        }
    }
}