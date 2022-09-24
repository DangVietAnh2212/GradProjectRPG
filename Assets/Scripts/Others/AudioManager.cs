using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Cannot find {sound} sound!!!");
            return;
        }
        sound.source.Play();
    }

    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
}
