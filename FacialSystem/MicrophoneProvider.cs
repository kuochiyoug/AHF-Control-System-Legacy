using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneProvider : MonoBehaviour
{
    public AudioSource source;
    public string[] mic;
    public int select = 0;

    private int prevSelect;


    // Start is called before the first frame update
    void Start()
    {
        source.clip = null;
    }

    // Update is called once per frame
    void Update()
    {
        // if we substitute source.clip in Start(), the sound gets latency. I don't know why.
        if (source.clip == null)
        {
            mic = Microphone.devices;
            source.clip = Microphone.Start(mic[select], true, 1, 44100);
            source.loop = true;
            while (!(Microphone.GetPosition(mic[select]) > 0)) { }
            source.Play();
            prevSelect = select;
        }

        if (prevSelect != select)
        {
            if (select < mic.Length)
            {
                Restart();
                prevSelect = select;
            }
            else
            {
                select = prevSelect;
            }
        }
    }

    [ContextMenu("Restart audio")]
    public void Restart()
    {
        Microphone.End(mic[prevSelect]);
        source.clip = Microphone.Start(mic[select], true, 60, 44100);
        source.loop = true;
        while (!(Microphone.GetPosition(mic[select]) > 0)) { }
        source.Play();
    }
}
