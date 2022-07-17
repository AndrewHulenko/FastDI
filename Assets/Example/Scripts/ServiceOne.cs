using FastDI;
using UnityEngine;

namespace Example
{
    public class ServiceOne : MonoBehaviour
    {
        [Inject] private ServiceTwo _serviceTwo;
        [Inject] private IServiceThree _serviceThree;

        private void Awake()
        {
            Binder.Install(this);
        }

        private void Start()
        {
            _serviceTwo.DoSomething();
            _serviceThree.DoSomething();
        }

        private void OnDestroy()
        {
            Binder.Remove(this);
        }
    }
}