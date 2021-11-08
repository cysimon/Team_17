using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent
{
    public int index;
    public bool new_player;

    public AudioEvent(int _in, bool _new_in)
    {
        index = _in;
        new_player = _new_in;
    }
}

public class AudioController : MonoBehaviour
{
    // 0 - Normal Loop
    // 1 - Battle Loop
    public AudioClip[] audios;
    AudioSource audioPlayer;
    Subscription<AudioEvent> subscription;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        subscription = EventBus.Subscribe<AudioEvent>(Listener);
    }

    void Listener(AudioEvent event_in)
    {
        if (event_in.new_player)
        {
            audioPlayer.clip = audios[event_in.index];
        }
        else
        {
            GameObject temp = new GameObject("Temp Clip");
            AudioSource temp_source = temp.AddComponent<AudioSource>();
            temp_source.clip = audios[event_in.index];
            temp_source.spatialBlend = 0.0f;
            temp_source.Play();
            Destroy(temp, audios[event_in.index].length);
        }
        
    }
}
