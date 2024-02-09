using Live2D.Cubism.Framework.MouthMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSourceSwitcher : MonoBehaviour
{
    [SerializeField] private Toggle sourceToggle;
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private AudioSource micSource;
    private CubismAudioMouthInput mouth;

    [SerializeField] bool isMic;

    // Start is called before the first frame update
    void Start()
    {
        mouth = GetComponent<CubismAudioMouthInput>();
        SwitchSoundSource(isMic);
        sourceToggle.isOn = isMic;
    }

    public void SwitchSoundSource(bool isMic)
    {
        this.isMic = isMic;
        if (this.isMic) 
        {
            mouth.AudioInput = micSource;
        }
        else
        {
            mouth.AudioInput = voiceSource;
        }
    }
}
