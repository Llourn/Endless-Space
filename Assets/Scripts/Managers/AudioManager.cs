using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;


public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public AudioMixer audioMixer;

    public AudioMixerSnapshot muteMusic;
    public AudioMixerSnapshot standardLevels;
    public AudioMixerSnapshot paused;
    [HideInInspector]
    public bool usingPauseSnapshot = false;

    private void Awake()
    {
        if (PlayerPrefsManager.GetLaunchedBefore() != 1)
        {
            PlayerPrefsManager.SetMusicVolume(0.5f);
            PlayerPrefsManager.SetSFXVolume(0.5f);
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = s.channel;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        
        audioMixer.SetFloat("musicVolume", Mathf.Log(PlayerPrefsManager.GetMusicVolume()) * 20);
        audioMixer.SetFloat("sfxVolume", Mathf.Log(PlayerPrefsManager.GetSFXVolume()) * 20);
    }

    private void Update()
    {
        if(Time.timeScale == 0)
        {
            usingPauseSnapshot = true;
            paused.TransitionTo(0.01f);
        }
        
        if(Time.timeScale != 0 && usingPauseSnapshot)
        {
            usingPauseSnapshot = false;
            standardLevels.TransitionTo(0.1f);
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.Stop();
    }

    public void FadeStandardLevelsIn(float time)
    {
        if(time >= 0.0f)
        {
            standardLevels.TransitionTo(time);
        }
        else if(time < 0.0f)
        {
            StartCoroutine(StartGameMusicFadeIn());
        }
    }

    public void FadeMusicOut(float time)
    {
        muteMusic.TransitionTo(time);
    }

    IEnumerator StartGameMusicFadeIn()
    {
        yield return null;
        FadeStandardLevelsIn(1.5f);
    }

    public void StopAllMusic()
    {
        Stop("Music Menu");
        Stop("Music Game");
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefsManager.SetMusicVolume(volume);
        audioMixer.SetFloat("musicVolume", Mathf.Log(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefsManager.SetSFXVolume(volume);
        audioMixer.SetFloat("sfxVolume", Mathf.Log(volume) * 20);
    }

    public void PauseAudioSnapshot()
    {
        usingPauseSnapshot = true;
        paused.TransitionTo(0.1f);
    }

}
