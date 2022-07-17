using UnityEngine;

namespace FastDI
{
    public static class Print
    {
        public static void Warning(string message, object instance = null)
        {
            MonoBehaviour monoBehaviour = instance as MonoBehaviour;
            GameObject gameObject = monoBehaviour != null ? monoBehaviour.gameObject : null;
            Debug.Log("<color=#e6ac2f><b>[ FastDI ] " + message + "</b></color>", gameObject);
        }

        public static void Error(string message, object instance = null)
        {
            MonoBehaviour monoBehaviour = instance as MonoBehaviour;
            GameObject gameObject = monoBehaviour != null ? monoBehaviour.gameObject : null;
            Debug.Log("<color=#e15656><b>[ FastDI ] " + message + "</b></color>", gameObject);
        }
    }
}