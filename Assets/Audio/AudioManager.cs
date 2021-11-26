using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public string currentMusic;
    public static bool musicEnabled = false;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void PlayMusic(string musicName)
    {
        if (musicEnabled)
        {
            currentMusic = musicName;
            PlaySound(musicName);
        }
    }

    public void PlaySound(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void StopSound(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
}
