using System.Collections.Concurrent;

namespace FastDI
{
    /// <summary>
    /// Version 1.0
    /// Author: Andrew Hulenko (https://github.com/AndrewHulenko/FastDI)
    /// FastDI is a thread-safe fast Dependency Injector for Unity
    /// </summary>
    
    public static class Binder
    {
        private static readonly ConcurrentDictionary<Context, Container> Containers = new ConcurrentDictionary<Context, Container>();
        
        /// <summary>
        /// Install all dependencies in Awake()
        /// Be note. Any call to injected reference should be in the Start() method or higher
        /// because the order of initialization is not guaranteed on different MonoBehaviour objects.
        /// </summary>
        public static void Install(object instance, Context context = Context.Global)
        {
            if(!Containers.TryGetValue(context, out Container container))
            {
                container = new Container(context);
                if (!Containers.TryAdd(context, container))
                {
                    Print.Error("Can't create container in context: " + context, instance);
                    return;
                }
            }
            
            container.Install(instance);
        }

        /// <summary>
        /// Remove should be in the OnDestroy().
        /// </summary>
        public static void Remove(object instance, Context context = Context.Global)
        {
            if (!Containers.TryGetValue(context, out Container container))
            {
                Print.Error("Can't get container in context: " + context, instance);
                return;
            }

            container.Remove(instance);
        }
    }
}