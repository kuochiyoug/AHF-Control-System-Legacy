using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Linq;

public class CsvReadWrite : MonoBehaviour
{

    private List<string[]> rowData = new List<string[]>();
    private bool Record_trigger = false;
    private bool Ready = false;
    private StreamWriter DataFile;


    public readonly string delimiter = ",";

    private void Awake()
    {
    }

    // Use this for initialization
    public void CreateLogFile(string fileName = "Saved_Data.csv")
    {
        string folderPath = getPath();
        string filePath = folderPath + fileName;


        if (!System.IO.Directory.Exists(folderPath))
        {
            Debug.Log("No folder exists! Creating: " + folderPath);
            System.IO.Directory.CreateDirectory(folderPath);
        }

        DataFile = System.IO.File.CreateText(filePath);
        Debug.Log("File created: " + filePath);

        CloseFile();
    }
        
    public void WriteFloatArray(double[] array)
    {
        StringBuilder sb = new StringBuilder();
        string[] str_array = array.OfType<object>().Select(o => o.ToString()).ToArray();
        sb.AppendLine(string.Join(delimiter, str_array));
        DataFile.WriteLine(sb);
    }
    public bool StartRecord()
    {
        try
        {
            Record_trigger = true;
            return true;
        }
        catch(ArithmeticException e)
        {
            return false;
        }
    }
    public bool StopRecord()
    {
        try
        {
            Record_trigger = false;
            return true;
        }
        catch (ArithmeticException e)
        {
            return false;
        }
    }
    public void CloseFile()
    {
        if (DataFile != null)
        {
            DataFile.Close();
        }
    }

    // Following method is used to retrive the relative path as device platform
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

}
