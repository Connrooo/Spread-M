using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Sound[] sounds;
    public Sound[] walkSounds;
    public Sound[] music;
    public static AudioManager Instance { get; private set; }
    //public AudioSource MusicSource;
    GameManagerStateMachine gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)

        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.panStereo = s.direction;
            s.audioSource.outputAudioMixerGroup = s.mixerGroup;
            
        }
        foreach (Sound s in walkSounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.panStereo = s.direction;
            s.audioSource.outputAudioMixerGroup = s.mixerGroup;
        }
        foreach (Sound s in music)
        {
            s.audioSource = s.audioSource.gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.panStereo = s.direction;
            s.audioSource.outputAudioMixerGroup = s.mixerGroup;
        }
    }
    private void Start()
    {
        foreach (Sound s in sounds)
        {
            if (s.audioSource.playOnAwake)
            {
                s.audioSource.Play();
            }
        }
        foreach (Sound s in music)
        {
            s.audioSource.Play();
        }
    }


    private void Update()
    {
        HandleMusic();
    }


    public void Play(string name)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }
    public void PlayWalk()
    {
        try
        {
            Sound s = walkSounds[UnityEngine.Random.Range(0, walkSounds.Length)];
            s.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    public void Stop(string name)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.audioSource.Stop();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    private void HandleMusic()
    {
        if (gameManager._Paused)
        {
            if (music[1].audioSource.volume != 0)
            {
                music[1].audioSource.volume -= Time.deltaTime/2;
                music[0].audioSource.volume += Time.deltaTime/2;
            }
            else
            {
                music[0].audioSource.volume = .2f;
            }
        }
        else
        {
            if (music[0].audioSource.volume != 0)
            {
                music[0].audioSource.volume -= Time.deltaTime/2;
                music[1].audioSource.volume += Time.deltaTime/2;
            }
            else
            {
                music[1].audioSource.volume = .2f;
            }
        }
    }
}