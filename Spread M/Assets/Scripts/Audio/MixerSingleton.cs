using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MixerSingleton : MonoBehaviour
{
    public static MixerSingleton Instance;


    public AudioMixer singletonMixer;

    public AudioMixer Mixer { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
    }
}
