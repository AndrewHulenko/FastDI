using FastDI;
using System;
using UnityEngine;

namespace Example
{
    public class ServiceThree : IServiceThree, IDisposable
    {
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