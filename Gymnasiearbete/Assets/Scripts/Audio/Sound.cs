using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioMixer mixer;

    public string name;

    public bool loopable;

    public AudioClip audioClip;

    [HideInInspector]
    public AudioSource audioSource;
}
