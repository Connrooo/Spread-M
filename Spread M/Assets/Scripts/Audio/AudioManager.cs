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
    public Sound[] groanSounds;
    public Sound[] infectSounds;
    public Sound[] screamSounds;
    public Sound[] attackedSounds;
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

        LoadSoundVariables(sounds);
        LoadSoundVariables(walkSounds);
        LoadSoundVariables(groanSounds);
        LoadSoundVariables(infectSounds);
        LoadSoundVariables(screamSounds);
        LoadSoundVariables(attackedSounds);
        LoadSoundVariables(music);
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
    private AudioClip PlayOneShotClip(AudioSource source,Sound s, AudioClip currentClip)
    {
        var clip = s.clip;
        if (currentClip != clip) 
        {
            source.Stop(); 
            currentClip = clip;
            source.PlayOneShot(clip,s.volume);
        }
        else
        {//otherwise, it checks if the src is currently playing the audioclip and plays it if it isn't
            if (!source.isPlaying)
            {
                source.PlayOneShot(clip,s.volume);
            }
        }
        return currentClip;
    }
    public void PlayWalk(AudioSource source)
    {
        try
        {
            Sound s = walkSounds[UnityEngine.Random.Range(0, walkSounds.Length)];
            source.PlayOneShot(s.clip,s.volume);
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }
    public AudioClip PlayInfect(AudioSource source,AudioClip currentClip)
    {
        try
        {
            Sound s = infectSounds[UnityEngine.Random.Range(0, infectSounds.Length)];
            PlayOneShotClip(source, s, currentClip);
            return currentClip;
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }
    public AudioClip PlayScreamOrGroan(AudioSource source, AudioClip currentClip, bool infected)
    {
        try
        {
            Sound s = new();
            if (infected)
            {
                s = groanSounds[UnityEngine.Random.Range(0, groanSounds.Length)];
            }
            else
            {
                s = attackedSounds[UnityEngine.Random.Range(0, attackedSounds.Length)];
            }
            source.PlayOneShot(s.clip, s.volume);
            return currentClip;
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }
    public AudioClip PlayAttacked(AudioSource source, AudioClip currentClip)
    {
        try
        {
            Sound s = attackedSounds[UnityEngine.Random.Range(0, attackedSounds.Length)];
            PlayOneShotClip(source, s, currentClip);
            return currentClip;
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
    private void LoadSoundVariables(Sound[] loadedSound)
    {
        foreach (Sound s in loadedSound)
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
    }
}