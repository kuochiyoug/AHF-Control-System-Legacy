using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CutieroidUtility;
using UnityEngine.SceneManagement;
using System.Text;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;

public class MotionAndFacialAPI : MonoBehaviour
{

    public MainManager SceneComminicator;
    public MotionPlayer motionPlayer;
    public uint frames;
    public uint current_frame;
    public bool has_motion;
    public Dropdown ListFiles;
    public Button SaveAudioAndSoundButton;
    public Button RecordButton;
    public Button PlayAllButton;
    public AutoMotionPlayer am;


    public bool has_recored_facial = false;
    public bool has_recored_sound = false;

    private bool Recording;
    private string[] facial_animation_command_array;
    private string[] sound_command_array;
    private StreamWriter DataFile;
    private StringBuilder sb;
    //private MotionRecorder facialmotionRecorder;
    private float progress;
    private string prevMotionPlayerMotion;
    [SerializeField] private FacialControlAPI facialControlAPI;
    private bool CanRecord;

    private void Start()
    {
        Button btnSave = SaveAudioAndSoundButton.GetComponent<Button>();
        btnSave.onClick.AddListener(SaveButton);
        Button btnRecord = RecordButton.GetComponent<Button>();
        btnRecord.onClick.AddListener(RecordFacialAndSound);
        am = GetComponent<AutoMotionPlayer>();
        //Dropdown myFiles = ListFiles.GetComponent<Dropdown>();
        ListFiles.onValueChanged.AddListener(delegate {
            LoadThisFile(ListFiles);
        });
        Button btnPlay = PlayAllButton.GetComponent<Button>();
        btnPlay.onClick.AddListener(PlayAll);
        Recording = false;
        has_motion = false;
        CanRecord = false;
    }

    private void PlayAll()
    {
        motionPlayer.PlayMotion();
        LinkUpMotionFacial();
        string items = "";
        foreach (var item in facial_animation_command_array)
            items = item + "," + items;
        Debug.Log("MotionAndFacial: Array=" + string.Join(", ", facial_animation_command_array));
    }

    public void LinkUpMotionFacial()
    {
        frames = motionPlayer.frames;
        has_motion = true;
        facialControlAPI = SceneComminicator.FacialControlAPI;
        string facial_filename = Path.GetFileNameWithoutExtension(motionPlayer.Motion_file) + "_Facial" + ".facialmotion";
        string facial_filepath = Path.Combine(Path.GetDirectoryName(motionPlayer.Motion_file), facial_filename);

        string sound_command_filename = Path.GetFileNameWithoutExtension(motionPlayer.Motion_file) + "_Sound" + ".soundmotion";
        string sound_command_filepath = Path.Combine(Path.GetDirectoryName(motionPlayer.Motion_file), sound_command_filename);

        facial_animation_command_array = new string[frames]; // Prepare Facial Motion Embbeding Data
        sound_command_array = new string[frames];
        Debug.Log("Get Motion " + motionPlayer.Motion_file + " it has " + frames + " frames");

        if (File.Exists(facial_filepath))
        {
            //FacialMotion facial_file = new FacialMotion(facial_filepath);
            Debug.Log("It has facial file! Load " + facial_filename);
            facial_animation_command_array = System.IO.File.ReadAllLines(facial_filepath);
            has_recored_facial = true;
        }
        else
        {
            Debug.LogWarning("No Facial file.");
        }


        if (File.Exists(sound_command_filepath))
        {
            //FacialMotion facial_file = new FacialMotion(sound_command_filepath);
            Debug.Log("It has sound file! Load " + sound_command_filename);
            sound_command_array = System.IO.File.ReadAllLines(sound_command_filepath);
            has_recored_sound = true;
        }
        else
        {
            Debug.LogWarning("No Sound file.");
        }
    }

    private void LoadThisFile(Dropdown change)
    {
        CanRecord = true;
        string thisFile = am.MotionFiles[change.value];
        Debug.Log("MotionAndFacial: Loading File " + thisFile);
        if (thisFile == null)
            return;
        motionPlayer?.ReadMotion(thisFile);
    }

    private void SaveButton()
    {
        string thisFile = motionPlayer.Motion_file;
        string items = "";

        if (facial_animation_command_array == null)
        {
            LinkUpMotionFacial();
        }
        foreach (var item in facial_animation_command_array)
            items = items + "," + item;
        Debug.Log(items);
        string thiPath = Path.GetDirectoryName(thisFile);
        thisFile = Path.GetFileNameWithoutExtension(thisFile);
        //SaveAnimation;
        System.IO.File.WriteAllLines(Path.Combine(thiPath, string.Format("{0}_Facial.facialmotion", thisFile)), facial_animation_command_array);
        //SaveSoundArray;
        System.IO.File.WriteAllLines(Path.Combine(thiPath, string.Format("{0}_Sound.soundmotion", thisFile)), sound_command_array);
    }

    private void Update()
    {
        if (motionPlayer == null)
        {
            motionPlayer = SceneComminicator.MotionPlayer;
        }
        else
        {
            if (motionPlayer.has_motion && prevMotionPlayerMotion != motionPlayer.Loaded_motion)
            {
                LinkUpMotionFacial();
            }
            if (motionPlayer.has_motion && !Recording)
            {
                current_frame = motionPlayer.current_frame;
                if (current_frame < motionPlayer.frames)
                {
                    if (has_recored_facial)
                    {
                        if (current_frame > facial_animation_command_array.LongLength)
                        {
                            Debug.LogWarning("Current Frame is greater than existed facial length! Please Check!!!!");
                            current_frame = (uint)facial_animation_command_array.LongLength -1 ;
                        }
                        if (facial_animation_command_array[current_frame] != "" && facial_animation_command_array[current_frame] != null)
                        {
                            facialControlAPI.PlayAnimation(facial_animation_command_array[current_frame]);
                            Debug.Log("Facial Animation " + facial_animation_command_array[current_frame] + " at frame " + current_frame);
                        }
                    }
                    if (has_recored_sound)
                    {
                        if (current_frame > sound_command_array.LongLength)
                        {
                            Debug.LogWarning("Current Frame is greater than existed sound length! Please Check!!!!");
                            current_frame = (uint)sound_command_array.LongLength - 1;
                        }
                        if (sound_command_array[current_frame] != "" && sound_command_array[current_frame] != null)
                        {
                            //facialControlAPI.SayVoiceClass(sound_command_array[current_frame]);
                            facialControlAPI.SayVoice(sound_command_array[current_frame]);
                            Debug.Log("Sound " + sound_command_array[current_frame] + " at frame " + current_frame);
                        }
                    }
                }
            }
        }
        if (motionPlayer != null)
            prevMotionPlayerMotion = motionPlayer.Loaded_motion;
    }

    private void WriteStringArray(string[] wList)
    {
        sb.Clear();
        string[] str_array = wList.OfType<object>().Select(o => o.ToString()).ToArray();
        sb.AppendLine(string.Join(",", str_array));
        DataFile.Write(sb);
    }

    void AddFacialAnimation(uint place_frame)
    {
        facial_animation_command_array[place_frame] = facialControlAPI.onGoingAnim;
    }

    void AddSoundCommand(uint place_frame)
    {
        sound_command_array[place_frame] = facialControlAPI.onGoingVoice;
        //sound_command_array[place_frame] = facialControlAPI.VoiceClassCommand;
    }

    public void SetFramePoint(uint point_frame)
    {
        motionPlayer.current_frame = point_frame;
    }

    public void PauseMotionPlayer()
    {
        motionPlayer.Pause();
    }

    public void RecordFacialAndSound()
    {
        if (CanRecord)
        {
            if (Recording)
            {
                RecordButton.GetComponentInChildren<Text>().text = "Record Facial";
                Debug.Log("Stop Record Facial Animation");
                Debug.Log("Stop Record Sound Animation");
            }
            else
            {
                RecordButton.GetComponentInChildren<Text>().text = "Stop Record";
                Debug.Log("Start Record Facial Animation");
                Debug.Log("Start Record Sound Animation");
                StartCoroutine(EnumRecordFaceAudio());
            }
            Recording = !Recording;
        }
    }

    IEnumerator EnumRecordFaceAudio()
    {
        yield return new WaitForSeconds(0.1f);
        while (Recording)
        {
            current_frame = motionPlayer.current_frame;
            if (facialControlAPI.onGoingAnim != "")
            {
                AddFacialAnimation(current_frame);
                //Debug.Log("Add Facial Animation " + animation + " to frame " + current_frame);
            }
            if (facialControlAPI.onGoingVoice != "")
            {
                AddSoundCommand(current_frame);
                //Debug.Log("Add Sound " + facialControlAPI.onGoingVoice + " to frame " + current_frame);
            }
            yield return new WaitForSeconds(0.01f);

        }
    }
}


public class FacialMotion
{
    public double FrameRate;
    public uint Frames;
    private char delimiter = ',';

    private uint MotionStartIndex;
    private uint MotionEndIndex;
    private string[] DataLines;

    public string[][] MotionArray;

    public FacialMotion(string FilePath)
    {
        DataLines = File.ReadAllLines(FilePath);
        GetDataDescription(DataLines);

        string[] MotionArrayString = new List<string>(DataLines).GetRange((int)MotionStartIndex, (int)Frames).ToArray();

        List<string[]> MotionArrayList = new List<string[]>();
        for (int i = 0; i < Frames; i++)
        {
            string[] linedatastr = MotionArrayString[i].Split(delimiter);
            MotionArrayList.Add(linedatastr);
        }
        MotionArray = MotionArrayList.ToArray();
    }

    private void GetDataDescription(string[] lines)
    {
        for (int i = 0; i < DataLines.Length; i++)
        {
            if (DataLines[i].Contains("MOTIONS:"))
            {
                MotionStartIndex = (uint)i + 1;
            }
            if (DataLines[i].Contains("END"))
            {
                MotionEndIndex = (uint)i - 1;
            }
        }
        Frames = MotionEndIndex - MotionStartIndex + 1;
        //MotionArray = new double[Frames][ DOF * ValuePerDOF];
    }

}