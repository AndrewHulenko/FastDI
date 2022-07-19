## Fast Dependency Injection for Unity

[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

### How it looks
<img src="FastDI.gif" alt="FastDI" width="700px" />

### Download
[Unity Packages](https://github.com/AndrewHulenko/FastDI/releases/tag/FastDI)


### Support injection by:
- MonoBehaviour classes
- Regular classes
- Interfaces

### Features
- Fast
- Lightweight
- Thread-safe
- Production tested
- Supports all versions of Unity
- Different contexts (Global, Scene...)

### Code example
More details can be found in the [Document](Assets/FastDI/Docs/Description.pdf)

``` csharp
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