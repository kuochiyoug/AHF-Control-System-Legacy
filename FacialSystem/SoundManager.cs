using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    List<AudioClip> AudioClipsList;
    public AudioClip[] resourceAudioClips;
    private AudioSource VoiceSource;

    [HideInInspector]
    public bool isPlaying = false;

    public string CurrentLoadedAudioClip = "";

    public List<string> Voice_class;
    public List<int> Voice_list;
    private Dictionary<string, int> keyVoice_class;

    public Dropdown dropdown;



    //private Random random;
    private void Awake()
    {
        Debug.Log("[SoundManager] searching voice...");
        resourceAudioClips = Resources.LoadAll<AudioClip>("Sounds/Voice/");
        Debug.Log("[SoundManager] "+ resourceAudioClips.Length + " voices has found.");
        VoiceSource = GetComponent<AudioSource>();
        //random = new Random();
        Voice_class = new List<string>();
        keyVoice_class = new Dictionary<string, int>();
        //for (int i = 0; i < resourceAudioClips.Length; i++)
        int idx = 0;
        foreach (AudioClip clip in resourceAudioClips)
        {
            string class_name = clip.name.Split('_')[0];
            keyVoice_class.Add(clip.name, idx);
            if (!Voice_class.Contains(class_name))
            {
                Voice_class.Add(class_name);
            }
            idx++;
        }
    }

    private void Start()
    {
        dropdown.onValueChanged.AddListener(AnyClassVoicePlay);
        dropdown.ClearOptions();
        dropdown.AddOptions(Voice_class);
    }

    private void AnyClassVoicePlay(int index)
    {
        SpeakRandomInClass(Voice_class[index]);
    }

    public bool SpeakRandomInClass(string class_name)
    {
        if (!Voice_class.Contains(class_name))
            return false;

        Voice_list = new List<int>();
        int idx = 0;
        foreach (string clipname in keyVoice_class.Keys)
        {
            if (clipname.Contains(class_name))
            {
                Voice_list.Add(idx);
            }
            idx++;
        }

        int random_idx = UnityEngine.Random.Range(0, Voice_list.Count);
        Debug.Log(resourceAudioClips[Voice_list[random_idx]]);
        VoiceSource.clip = resourceAudioClips[Voice_list[random_idx]];
        VoiceSource.Play();
        return true;
    }

    public bool Speak(string clip_name)
    {
        if (!keyVoice_class.ContainsKey(clip_name))
        {
            Debug.LogWarning("[SoundManager] " + clip_name + " not in Audio list.");
            return false;
        }

        int idx = keyVoice_class[clip_name];
        Debug.Log(resourceAudioClips[idx]);
        VoiceSource.clip = resourceAudioClips[idx];
        VoiceSource.Play();
        return true;
    }

    public bool SpeakSomething()
    {

        int random_idx = UnityEngine.Random.Range(0, resourceAudioClips.Length);
        Debug.Log(resourceAudioClips[random_idx]);
        VoiceSource.clip = resourceAudioClips[random_idx];
        VoiceSource.Play();
        return true;
    }

    private void Update()
    {
        isPlaying = VoiceSource.isPlaying;
        if (VoiceSource.isPlaying)
        {
            CurrentLoadedAudioClip = VoiceSource.clip.name;
        }
        else
        {
            CurrentLoadedAudioClip = "";
        }
        
    }

}
