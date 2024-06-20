using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sounds")]
    public Sound[] Sounds;
    
    private void Awake() 
    {
        Instance = this;

        foreach (Sound s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.AudioClip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.mute = s.Mute;
            s.Source.loop = s.Loop; 
            s.Source.playOnAwake = s.playOnAwake;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound with name {name} not found in AudioManager.");
            return;
        }

        s.Source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound with name {name} not found in AudioManager.");
            return;
        }

        s.Source.Stop();
    }

    public void MuteAllSoundEffects()
    {
        foreach (Sound s in Sounds)
        {
            s.Source.mute = true;
        }
    }

    public void UnmuteAllSoundEffects()
    {
        foreach (Sound s in Sounds)
        {
            s.Source.mute = false;
        }
    }
}
