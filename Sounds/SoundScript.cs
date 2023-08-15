using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
//Class script for the sound properties 
public class SoundScript
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
