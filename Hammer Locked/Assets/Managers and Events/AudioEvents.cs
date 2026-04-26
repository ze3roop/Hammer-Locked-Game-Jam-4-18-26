using System;
using NUnit.Framework.Internal;
using UnityEngine;

public class AudioEvents
{
    public event Action<SoundData, Vector3> OnPlaySound; 
    public void PlaySound(SoundData sound, Vector3 position)
    {
        OnPlaySound?.Invoke(sound, position); 
    }
}
