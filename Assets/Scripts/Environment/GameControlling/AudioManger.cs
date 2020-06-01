using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManger : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManger instance;
    // Start is called before the first frame update
    void Awake()
    {

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Play("Theme", gameObject);
        //foreach (Sound s in sounds)
        //{
          //  s.source = gameObject.AddComponent<AudioSource>();
        //    s.source.clip = s.clip;
        //    s.source.volume = s.volume;
        //    s.source.pitch = s.pitch;
        //    s.source.loop = s.loop;
      //  }
    }

    public void Play (string name, GameObject obj)
    {
        Sound s=Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source = obj.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.Play();
    }
}
