using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public string currentMusic;
    public static bool musicEnabled = true;
    [SerializeField]
    private List<RandomSound> StartTableRandomMusic;

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

        PlayMusic("HumDrum", true, StartTableRandomMusic);
        //StartCoroutine(ClockTick());
    }

    [Serializable]
    public class RandomSound
    {
        public string name;
        public Sound.Type type;
    }

    private IEnumerator ClockTick()
    {
        yield return new WaitForSecondsRealtime(1);
        PlaySound("ClockTick");
        StopAllCoroutines();
        StartCoroutine(ClockTick());
    }

    public void PlayMusic(string musicName, bool playRandomAfter = false, List<RandomSound> possibleRandomSounds = null)
    {
        if (musicEnabled)
        {
            currentMusic = musicName;
            var musicSound = PlaySound(musicName);
            if (playRandomAfter && possibleRandomSounds.Any())
            {
                StartCoroutine(PlayRandomNextSound(possibleRandomSounds, musicSound.clip.length + 2, true, true));
            }
        }
    }

    public Sound PlaySound(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        return s;
    }

    public void StopSound(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    private IEnumerator PlayRandomNextSound(List<RandomSound> possibleSounds, float delay, bool stopCurrMusic = false, bool keepRandomingAfter = true)
    {
        yield return new WaitForSecondsRealtime(delay);
        if(stopCurrMusic) StopSound(currentMusic);
        var r = UnityEngine.Random.Range(0, possibleSounds.Count - 1);
        var sound = possibleSounds[r];
        if (sound.type == Sound.Type.Music)
            PlayMusic(sound.name, keepRandomingAfter, possibleSounds);
        else if (sound.type == Sound.Type.Sound)
            PlaySound(sound.name);
    }
}
