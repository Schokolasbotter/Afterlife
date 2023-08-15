using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public SoundScript[] sounds;
    public static AudioManagerScript instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //Don't destroy when moving through scenes
        DontDestroyOnLoad(gameObject);
        //For each sound create the following
        foreach(SoundScript s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    //Play background music on launch
    void Start()
    {
        Play("BackgroundMusic");
    }
    //Call "Play" and the name of the sound to play it
    public void Play(string name)
    {
        SoundScript s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            return;
        }
        s.source.Play();
    }

    public void changeVolume(string name, float changeValue)
    {
        SoundScript s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.volume += changeValue;
    }
}
