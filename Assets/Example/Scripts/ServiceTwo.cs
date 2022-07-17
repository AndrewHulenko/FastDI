using FastDI;
using UnityEngine;

namespace Example
{
    public class ServiceTwo : MonoBehaviour
    {
        private ServiceThree _serviceThree;
        
        public void DoSomething()
        {
            Debug.Log("Inject MonoBehaviour - Works");
        }
        
        private void Awake()
        {
            Binder.Install(this);
            _serviceThree = new ServiceThree();
        }

        private void OnDestroy()
        {
            _serviceThree.Dispose();
            Binder.Remove(this);
        }
    }
}