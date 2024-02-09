using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.LookAt;
using System.Security.Policy;
using UnityEngine.SceneManagement;
using System;

public class FacialControlAPI : MonoBehaviour
{
    public enum emoteType
    {
        Idle
    }

    private Animator anim;
    private CubismAutoEyeBlinkInput blinkcontrol;
    private CubismLookController lookcontrol;
    private AudioSource VoiceSource;
    //public HeadMotion headMotion;

    public LookTarget lookTarget;
    public SoundManager soundManager;
    public string[] ListFiles;

    public string onGoingAnim;
    public string onGoingVoice;
    public string VoiceClassCommand;

    private Coroutine lookAnimationLoop;


    // Start is called before the first frame update
    void Awake()
    {
        onGoingAnim = "";
        onGoingVoice = "";
        VoiceClassCommand = "";

        anim = GetComponent<Animator>();
        blinkcontrol = GetComponent<CubismAutoEyeBlinkInput>();
        lookcontrol = GetComponent<CubismLookController>();
        VoiceSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
#if UNITY_EDITOR
#endif
        //var motorControlScene = SceneManager.GetSceneByName("MotorControlPanel");
        //if (motorControlScene != null)
        //{
        //    foreach (var go in motorControlScene.GetRootGameObjects())
        //    {
        //        headMotion = go.GetComponent<HeadMotion>();
        //        if (headMotion)
        //        {
        //            break;
        //        }
        //    }
        //}
        //else
        //{
        //    print("FacialSystem couldn't load headmotion");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        onGoingVoice = soundManager.CurrentLoadedAudioClip;
        AnimationUpdate();
        SpeakUpdate();
    }

    private void LateUpdate()
    {
        
    }


    void SpeakUpdate()
    {
        string VoiceClassCommand = "";
        //VoiceClassCommand = "";
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            VoiceClassCommand = "Idle";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            VoiceClassCommand = "happy";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            VoiceClassCommand = "angry";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            VoiceClassCommand = "annoy";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            VoiceClassCommand = "joy";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            VoiceClassCommand = "XFaceBad";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha6))
        {
            VoiceClassCommand = "shy";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha8))
        {
            VoiceClassCommand = "Smile";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.H))
        {
            VoiceClassCommand = "harasment";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha9))
        {
            VoiceClassCommand = "goodday";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F1))
        {
            VoiceClassCommand = "intro";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F2))
        {
            VoiceClassCommand = "thank";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F3))
        {
            VoiceClassCommand = "pround";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F4))
        {
            VoiceClassCommand = "question";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F5))
        {
            VoiceClassCommand = "stageHi";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F6))
        {
            VoiceClassCommand = "hate";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F7))
        {
            VoiceClassCommand = "doya";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            VoiceClassCommand = "random";
        }

        if (VoiceClassCommand != "")
        {
            if (VoiceClassCommand == "random")
            {
                SaySomething();
            }
            else
            {
                SayVoiceClass(VoiceClassCommand);
            }
            
            print(VoiceClassCommand);
        }
    }

    void AnimationUpdate()
    {
        onGoingAnim = "";
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            onGoingAnim = "IDLE";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            onGoingAnim = "HappyFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            onGoingAnim = "AngryFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            onGoingAnim = "SadFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            onGoingAnim = "ExcitiedFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            onGoingAnim = "XFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha6))
        {
            onGoingAnim = "CheekyFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha8))
        {
            onGoingAnim = "HappyFaceE";
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.H))
        {
            onGoingAnim = "AngryFace";
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha9))
        {
            onGoingAnim = "HappyFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F4))
        {
            onGoingAnim = "QuestionFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F1))
        {
            onGoingAnim = "HappyFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F5))
        {
            onGoingAnim = "HappyFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F6))
        {
            onGoingAnim = "AngryFace";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F2))
        {
            onGoingAnim = "HappyFaceE";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F3))
        {
            onGoingAnim = "HappyFaceE";
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F7))
        {
            onGoingAnim = "Doya";
        }

        if (onGoingAnim != "")
        {
            PlayAnimation(onGoingAnim);
            print(onGoingAnim);
            foreach (string thisfile in ListFiles)
            {
                Debug.Log(thisfile);
            }
        }
    }

    public void PlayAnimation(string animation_name)
    {
        Debug.Log("[FacialAPI] gat animation_name " + animation_name);

        //~~~~~~~~~~For MV Dance Robot Dance~~~~~~~~~~
        if (animation_name == "LookUp")
        {
            LookAnimation(Vector2.up, 3);
            return;
        }
        else if (animation_name == "LookDown")
        {
            LookAnimation(Vector2.down, 3);
            return;
        }
        else if (animation_name == "LookRight")
        {
            LookAnimation(Vector2.right, 3);
            return;
        }
        else if (animation_name == "LookLeft")
        {
            LookAnimation(Vector2.left, 3);
            return;
        }
        else
        {
            EndLookAnimation();
        }
        //^^^^^^^^^^For MV Dance Robot Dance^^^^^^^^^^

        if (animation_name == "Smile")
        {
            animation_name = "_Smile 0";
        }
        else if (animation_name == "Doya")
        {
            animation_name = "_Doya";
        }
        else if(animation_name== "XFaceBad")
        {
            animation_name = "XFace";
        }
        else if(animation_name == "Exciting")
        {
            animation_name = "ExcitedFace";
        }
        //else if(animation_name != "IDLE")  // Warning ! 
        //{
        //    animation_name += "Face";
        //}

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animation_name))
        {
            return;
        }
        Debug.Log("[Facial API] Play " + animation_name);
        anim.Play(animation_name, -1);
    }
    public void EyeControlOff()
    {
        blinkcontrol.enabled = false;
        lookcontrol.enabled = false;
    }
    public void EyeControlOn()
    {
        blinkcontrol.enabled = true;
        lookcontrol.enabled = true;
    }

    private void LookAnimation(Vector2 direction, float animationLength)
    {
        EndLookAnimation();

        lookAnimationLoop = StartCoroutine(LookLoop(direction, animationLength));
    }

    private void EndLookAnimation()
    {
        if (lookAnimationLoop != null)
        {
            StopCoroutine(lookAnimationLoop);
        }
        lookTarget?.StopAnimationOverride();
    }

    private IEnumerator LookLoop(Vector2 direction, float animationLength)
    {
        lookTarget.AnimationOverride(direction);

        yield return new WaitForSeconds(animationLength);

        lookTarget.StopAnimationOverride();
    }

    public void SayVoiceClass(string voice_class)
    {
        //VoiceSource.clip = audio;
        if (soundManager != null)
        {
            if (!soundManager.isPlaying)
            {
                soundManager.SpeakRandomInClass(voice_class);
            }
            else
            {
                Debug.LogWarning("[FacialContorlAPI] SoundManager is Playing Clips, Voice command interrupted.");
            }
        }
        else
        {
            Debug.LogWarning("SoundManager is not exist!");
        }
    }

    public void SayVoice(string voice_clip)
    {
        //VoiceSource.clip = audio;
        if (soundManager != null)
        {
            if (!soundManager.isPlaying)
            {
                soundManager.Speak(voice_clip);
                onGoingVoice = soundManager.CurrentLoadedAudioClip;
                //VoiceClassCommand = soundManager.CurrentLoadedAudioClip;
            }
            else
            {
                Debug.LogWarning("[FacialContorlAPI] SoundManager is Playing Clips, Voice command interrupted.");
            }
        }
        else
        {
            Debug.LogWarning("[FacialContorlAPI] SoundManager is not exist!");
        }
    }

    public void SaySomething()
    {
        //VoiceSource.clip = audio;
        if (soundManager != null)
        {
            if (!soundManager.isPlaying)
            {
                soundManager.SpeakSomething();
            }
            else
            {
                Debug.LogWarning("[FacialContorlAPI] SoundManager is Playing Clips, Voice command interrupted.");
            }
        }
        else
        {
            Debug.LogWarning("SoundManager is not exist!");
        }
    }

}
