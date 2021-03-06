using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public enum Type { Sound, Music };
    public Type type;
    public AudioClip clip;
    [Range(0,1)]
    public float volume;
    [Range(.1f, 10f)]
    public float pitch;
    public bool loop = true;

    [HideInInspector]
    public AudioSource source;

}
