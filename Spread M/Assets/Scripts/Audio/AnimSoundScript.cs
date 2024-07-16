using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AnimSoundScript : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip currentClip;
    AudioManager audioManager;

    float timeToNextNoise;

    [SerializeField] HumanStateMachine humanStateMachine;
    [SerializeField] bool isPlayer;

    private void Start()
    {
        if (!TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioManager = FindObjectOfType<AudioManager>();
        audioSource.outputAudioMixerGroup = MixerSingleton.Instance.singletonMixer.FindMatchingGroups("SFX")[0];
        timeToNextNoise = Random.Range(7, 25);
    }

    private void Update()
    {
        RandomNoiseCountdown();
    }

    public void PlayInfectionSound()
    {
        audioManager.PlayInfect(audioSource, currentClip);
    }

    public void PlayWalkSound()
    {
        audioManager.PlayWalk(audioSource);
    }

    public void PlayScreamingOrGroaningSound()
    {
        if (isPlayer)
        {
            audioManager.PlayScreamOrGroan(audioSource, currentClip, true);
        }
        else
        {
            audioManager.PlayScreamOrGroan(audioSource, currentClip, humanStateMachine._IsInfected);
        }
    }

    public void PlayAttackedSound()
    {
        audioManager.PlayAttacked(audioSource, currentClip);
    }

    public void RandomNoiseCountdown()
    {
        if (timeToNextNoise <= 0)
        {
            timeToNextNoise = Random.Range(7, 25);
            PlayScreamingOrGroaningSound();
        }
        else
        {
            timeToNextNoise -= Time.deltaTime;
        }
    }


}
