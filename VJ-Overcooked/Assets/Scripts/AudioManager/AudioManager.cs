using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Clips[] sounds;
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Clips c in sounds) {
            c.source = gameObject.AddComponent<AudioSource>();
            c.source.clip = c.sound;
            c.source.volume = c.volume;
            c.source.pitch = c.pitch;
            c.source.loop = c.loop;
        }
    }

    public void play(string name) {
        Clips c = Array.Find(sounds, sound => sound.soundName == name);
        if (c != null) c.source.Play();

    }

    public void stop(string name)
    {
        Clips c = Array.Find(sounds, sound => sound.soundName == name);
        if (c != null) c.source.Stop();
    }
}
