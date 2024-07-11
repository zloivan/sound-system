# Unity Sound System

![Unity Sound System](https://img.shields.io/badge/unity-2021.1%2B-blue)

## Overview
The Unity Sound System is an efficient sound management solution that supports object pooling. This system helps manage audio playback with minimal overhead, ensuring optimal performance for your Unity projects.

## Features
- Object pooling for sound emitters
- Configurable sound data with looping and mixer group support
- Easy-to-use API for sound management
- Random pitch adjustment for sound variations
- Logging for debugging and performance monitoring

## Installation
To install the package, follow these steps:

1. Open your Unity project.
2. Open `Window > Package Manager`.
3. Click on the `+` button in the top left corner and select `Add package from git URL...`.
4. Enter the following URL: `https://github.com/zloivan/SoundService.git` and click `Add`.

## Usage
### Setup
1. Add the `SoundManager` prefab to your scene.
2. Configure the `SoundManager` in the Inspector, setting the pool capacity and maximum pool size.
3. Get reference to SoundManager where you want to use sounds (inject or wrap it to singleton).
4. Use provided Builder class to Play sounds, you need to provide it with SoundData instance


### Example 
You can find that example in optional Samples.
Here is a basic example of how to use the `SoundManager` and `SoundEmitter`:

```csharp
using UnityEngine;
using Utilities.SoundService.Runtime;
using Utilities.SoundService.Runtime.data;

public class SoundUserExample : MonoBehaviour
{
    [SerializeField]
    private SoundData _soundData;
    [SerializeField]
    private SoundData _ambient;
    [SerializeField]
    private SoundManager _soundManager;
    [SerializeField]
    private float _fireRate;

    private double _lastFireTime;

    private void Start()
    {
        _soundManager.CreateSound()
            .WithSoundData(_ambient)
            .Play();
    }
    
    private void Update()
    {
        if (!Input.GetKey(KeyCode.Space)) 
            return;
        if (!(Time.time > _lastFireTime + _fireRate)) 
            return;
        
        _lastFireTime = Time.time;
        Fire();
    }

    private void Fire()
    {
        _soundManager.CreateSound()
            .WithSoundData(_soundData)
            .WithRandomPitch()
            .Play();
    }
}
```


# Logs
To enable logs for this system, add new defined symbols in `PlayerSettings.Other.Scripting Define Symbols` add `DEBUG_SOUND_SERVICE`

# Documentation
For detailed documentation and examples, please refer to the official documentation.

# Contributing
Contributions are welcome! Please submit a pull request or open an issue to discuss any changes or improvements.