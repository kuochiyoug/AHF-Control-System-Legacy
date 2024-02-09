using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AutoMotionPlayer : MonoBehaviour
{
    public string MotionFileFolder = "";
    public List<string> MotionFiles;

    public List<string> MotionName;
    public List<string> MotionClass;
    private Dictionary<string, int> keyMotion;

    private FacialControlAPI faceapi;
    private MotionPlayer mp;
    private MotionAndFacialAPI MotionAndFacialAPI;
    public bool play = false;
    public PlayMode mode = PlayMode.Normal;
    public int motion_id;
    public bool repeat = true;
    public bool show_info = false;

    // Start is called before the first frame update
    void Start()
    {
        MotionAndFacialAPI = GetComponent<MotionAndFacialAPI>();
        //mp = MotionAndFacialAPI.motionPlayer;
        var initPath = System.IO.Path.Combine(Application.dataPath, MotionFileFolder);
        string[] files = System.IO.Directory.GetFiles(initPath, "*.cutiemotion");
        

        MotionFiles = files.OfType<string>().ToList();

        MotionListUpdate();
        
    }

    private void MotionListUpdate()
    {
        MotionClass = new List<string>();
        MotionName = new List<string>();
        keyMotion = new Dictionary<string, int>();

        int idx = 0;
        foreach (string motionfile in MotionFiles)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(motionfile);
            string class_name = filename.Split('_')[0];
            keyMotion.Add(filename, idx);
            if (!MotionClass.Contains(class_name))
            {
                MotionClass.Add(class_name);
                if (show_info)
                    Debug.Log("[AutoMotionPlayer] Found class " + class_name);
            }
            idx++;
            if (show_info)
            {
                print("[AutoMotionPlayer] Get " + filename);
            }
        }
    }


    string PickNextMotion()
    {
        if (MotionFiles.Count > 0)
        {
            if (mode == PlayMode.Normal)
            {
                motion_id = motion_id + 1;
                Debug.Log("[AutoMotionPlayer] motion_id: " + motion_id);
                if (motion_id == MotionFiles.Count && repeat)
                {
                    motion_id = 0;
                }
                else if (motion_id == MotionFiles.Count && !repeat)
                {
                    play = false;
                    Debug.Log("[AutoMotionPlayer] Disable");
                    return "";
                }
            }
            else if (mode == PlayMode.Random)
            {
                motion_id = Random.Range(0, MotionFiles.Count);
                Debug.Log("[AutoMotionPlayer] motion_id: " + motion_id);
            }
            else
            {
                Debug.Log("[AutoMotionPlayer] idling motion ");
                return PickRandomMotionInClass("Idle");
            }
            return MotionFiles[motion_id];

        }
        else
        {
            return "";
        }
        
    }

    string PickRandomMotionInClass(string class_name)
    {
        if (!MotionClass.Contains(class_name))
        {
            Debug.LogWarning("[AutoMotionPlayer] No motion class called "+class_name+". No motion will be performed.");
            return "";
        }
            
        List<int> PreLoadMotionList = new List<int>();

        int idx = 0;
        foreach (string motionname in keyMotion.Keys)
        {
            if (motionname.Contains(class_name))
            {
                PreLoadMotionList.Add(idx);
            }
            idx++;
        }

        int random_idx = UnityEngine.Random.Range(0, PreLoadMotionList.Count);
        while (random_idx == motion_id)
        {
            random_idx = UnityEngine.Random.Range(0, PreLoadMotionList.Count);
        }

        motion_id = random_idx;
        //Debug.Log(MotionFiles[PreLoadMotionList[random_idx]]);
        return MotionFiles[PreLoadMotionList[random_idx]];
    }

    public void Play()
    {
        if (play == false)
        {
            play = true;
            Debug.Log("[AutoMotionPlayer] Enable");
        }
        else
        {
            play = false;
            Debug.Log("[AutoMotionPlayer] Disable");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            if (MotionAndFacialAPI.current_frame == MotionAndFacialAPI.frames &&  !MotionAndFacialAPI.motionPlayer.MotionOnGoing)
            {
                string Nextmotion = PickNextMotion();
                if (Nextmotion != "")
                {
                    MotionAndFacialAPI.motionPlayer.ReadMotion(Nextmotion);
                    MotionAndFacialAPI.motionPlayer.PlayMotion();
                    Debug.Log("[AutoMotionPlayer] Now Playing "+ Nextmotion);
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftControl)&& Input.GetKeyDown(KeyCode.I))
        {
            if (mode != PlayMode.Idle)
            {
                MotionAndFacialAPI.motionPlayer.Pause();
                mode = PlayMode.Idle;
                string Nextmotion = PickNextMotion();
                MotionAndFacialAPI.motionPlayer.ReadMotion(Nextmotion);
                MotionAndFacialAPI.motionPlayer.PlayMotion();
                Debug.Log("[AutoMotionPlayer] Now Playing " + Nextmotion);
            }
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            if (mode != PlayMode.Random)
            {
                MotionAndFacialAPI.motionPlayer.Pause();
                mode = PlayMode.Random;
                string Nextmotion = PickNextMotion();
                MotionAndFacialAPI.motionPlayer.ReadMotion(Nextmotion);
                MotionAndFacialAPI.motionPlayer.PlayMotion();
                Debug.Log("[AutoMotionPlayer] Now Playing " + Nextmotion);
            }
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
        {
            if (mode != PlayMode.Normal)
            {
                MotionAndFacialAPI.motionPlayer.Pause();
                mode = PlayMode.Idle;
                string Nextmotion = PickNextMotion();
                MotionAndFacialAPI.motionPlayer.ReadMotion(Nextmotion);
                MotionAndFacialAPI.motionPlayer.PlayMotion();
                Debug.Log("[AutoMotionPlayer] Now Playing " + Nextmotion);
            }
        }
    }

    public enum PlayMode
    {
        Normal,
        Random,
        Idle
    }
}

