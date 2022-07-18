using System;
using FastDI;
using UnityEngine;

namespace Example
{
    public class ServiceThree : IServiceThree, IDisposable
    {
        [Inject] private ServiceTwo _serviceTwo;
        
        public void DoSomething()
        {
            Debug.Log("Inject normal class by interface - Works");
        }

        public ServiceThree()
        {
            Binder.Install(this);
        }
        
        public void Dispose()
        {
            Binder.Remove(this);
        }
    }
}