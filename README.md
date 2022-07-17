## Fast Dependency Injection for Unity

[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

### How it looks in code
<img src="Docs/FastDI.gif" alt="FastDI" width="700px" />


### Support injection by:
- MonoBehaviour classes
- Normal classes
- Interfaces

### Features
- Fast
- Light
- Thread-safe
- Production tested
- Supports all versions of Unity
- Different contexts (Global, Scene...)

### Code example

```
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
```