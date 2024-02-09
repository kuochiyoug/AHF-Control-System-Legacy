using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Linq;

public class MotionRecorder : MonoBehaviour
{

    private StreamWriter DataFile;
    private StringBuilder sb;
    public readonly string delimiter = ",";

    public bool IsReady { get; private set; } = false;
    public bool IsRecordTimeStamp { get; private set; } = false;


    // Use this for initialization
    public bool CreateLogFile(string filePath = "Saved_Data.cutiemotion")
    {
        //string folderPath = getPath();

        string lastFolderName = Path.GetDirectoryName(filePath);
        if (!System.IO.Directory.Exists(lastFolderName))
        {
            Debug.Log("[Recorder] No folder exists! Creating: " + lastFolderName);
            System.IO.Directory.CreateDirectory(lastFolderName);
        }

        DataFile = System.IO.File.CreateText(filePath);
        Debug.Log("[Recorder] File created: " + filePath);

        sb = new StringBuilder();
        return true;
    }

    public void CloseFile()
    {
        if (IsReady)
        {
            DataFile.Write("END");
        }
        if (DataFile != null)
        {
            DataFile.Close();
        }
    }

    public void WriteDataDescription(string[] JointNameLst, string[] DataDescription, double FrameRate, string GeneratedFrom, bool RecordTimeStamp = false)
    {
        if (IsReady)
        {
            Debug.LogError("[Recorder] Already have Data Description! Aborted!");
            return;
        }

        WriteLine("MOTORNAMELIST: ");
        WriteArray(JointNameLst);
        string timestamprecord;
        if (RecordTimeStamp) 
        { 
            timestamprecord="TRUE";
            IsRecordTimeStamp = true;
        } 
        else 
        { 
            timestamprecord = "FALSE";
            IsRecordTimeStamp = false;
        }
        //WriteLine("RecordTimeStamp:" + timestamprecord);
        WriteLine("DATA DESCRIPTSION:");
        WriteArray(DataDescription);
        WriteLine("FRAME RATE:" + FrameRate.ToString());
        WriteLine("GeneratedFrom:" + GeneratedFrom);
        WriteLine("Recored On: " + System.DateTime.Now);
        WriteLine("---------");
        WriteLine("MOTIONS:");
        IsReady = true;
    }

    //public void UseAsFacialAnimation()
    //{
    //    WriteString("DATA DESCRIPTSION: FACIAL");
    //    WriteString("---------");
    //    WriteString("MOTIONS:");
    //    IsReady = true;
    //}


    
    // Following method is used to retrive the relative path as device platform
    public void RecordOneFrame<T>(T[] DataArray)
    {
        if (!IsReady)
        {
            Debug.LogError("[Recorder] No Data Description! ");
            return;
        }

        String strforrecord;
        if (IsRecordTimeStamp)
        {
            strforrecord = GetRecordTimePassed().ToString() + delimiter + ArrayToString(DataArray);
        }
        else
        {
            strforrecord = ArrayToString(DataArray);
        }
       
        WriteLine(strforrecord);
    }

    private string getPath()
    {
        #if UNITY_EDITOR
        return Application.dataPath + "/CSV/";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
        #else
        return Application.dataPath +"/"+"Saved_data.csv";
        #endif
    }


    private String ArrayToString<T>(T[] array)
    {
        string[] str_array = array.OfType<object>().Select(o => o.ToString()).ToArray();
        String Str = string.Join(delimiter, str_array);
        return Str;
    }
    private void WriteString(String Str)
    {
        DataFile.Write(Str);
    }
    private void WriteLine(String Str)
    {
        sb.Clear();
        sb.AppendLine(Str);
        DataFile.Write(sb);
    }

    private void WriteArray<T>(T[] array, bool withTimeStamp= false)
    {
        string[] str_array = array.OfType<object>().Select(o => o.ToString()).ToArray();
        String strToAppend = string.Join(delimiter, str_array);
        WriteLine(strToAppend);
    }

    double lastrecordtime = -1;
    private double GetRecordDeltatime()
    {
        if (lastrecordtime == -1) return 0.0;
        double deltatime = Time.timeAsDouble - lastrecordtime;
        lastrecordtime = Time.timeAsDouble;
        return deltatime;
    }

    double startrecordtime = -1;
    private double GetRecordTimePassed()
    {
        if (startrecordtime == -1)
        {
            startrecordtime = Time.timeAsDouble;
            return 0.0;
        }
        return Time.timeAsDouble - startrecordtime;
        
    }

}
