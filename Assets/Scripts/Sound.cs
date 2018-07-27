using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;
    public AudioMixerGroup channel;
    public bool loop;

    [Range(0.0f, 1.0f)]
    public float volume;
    [Range(0.0f, 4.0f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

}
