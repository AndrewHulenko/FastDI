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
        private static readonly ConcurrentDictionary<BinderContext, BinderContainer> Containers = new ConcurrentDictionary<BinderContext, BinderContainer>();
        
        /// <summary>
        /// Install all dependencies in Awake()
        /// Be aware. Any call to injected reference should be in the Start() method or higher
        /// because the order of initialization is not guaranteed on different MonoBehaviour objects.
        /// </summary>
        
        
        public static void Install(object instance, BinderContext context = BinderContext.Global)
        {
            if(!Containers.TryGetValue(context, out BinderContainer container))
            {
                container = new BinderContainer(context);
                if (!Containers.TryAdd(context, container))
                {
                    BinderUtils.PrintError(BinderUtils.ErrorCantCreateContainer, "Can't create container in context: " + context, instance);
                    return;
                }
            }
            
            container.Install(instance);
        }

        /// <summary>
        /// Remove should be in the OnDestroy().
        /// </summary>
        public static void Remove(object instance, BinderContext context = BinderContext.Global)
        {
            if (!Containers.TryGetValue(context, out BinderContainer container))
            {
                BinderUtils.PrintError(BinderUtils.ErrorCantGetContainer, "Can't get container in context: " + context, instance);
                return;
            }

            container.Remove(instance);
        }
    }
}