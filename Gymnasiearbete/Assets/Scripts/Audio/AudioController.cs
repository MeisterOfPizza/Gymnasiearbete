using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ArenaShooter.Controllers;

class AudioController : Controller<AudioController>
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.loop = s.loopable;
            
        }
    }

    private void Start()
    {
        Play("music");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
            return;
        float volume;
        s.mixer.GetFloat("Volume",out volume);
        float volumeForAduio = 1;
        s.audioSource.volume = volumeForAduio;
        s.audioSource.Play();
    }
}
