using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightSaveLoader : MonoBehaviour
{
    [SerializeField] private string savePath = "BodyData/SavedBodyData.txt";

    [SerializeField] private ShoulderPositionEstimation estimation;

    // Start is called before the first frame update
    void Awake()
    {
        LoadBodyData();
    }

    public void LoadBodyData()
    {
        var info = new FileInfo(Application.dataPath + "/" + savePath);
        if (!info.Exists)
        {
            SaveBodyData();
            return;
        }
        BodyData data;
        using (var reader = new StreamReader(info.OpenRead()))
        {
            var json = reader.ReadToEnd();
            data = JsonUtility.FromJson<BodyData>(json);
        }


        estimation.heightOfBody = data.height;
        estimation.correctionRate = data.correctionRate;
    }

    public void SaveBodyData()
    {
        var data = new BodyData();
        data.height = estimation.heightOfBody;
        data.correctionRate = estimation.correctionRate;

        var json = JsonUtility.ToJson(data);
        var path = Application.dataPath + "/" + savePath;
        using (var writer = new StreamWriter(path, false))
        {
            writer.WriteLine(json);
        }
    }

    private void OnApplicationQuit()
    {
        SaveBodyData();
    }
}
[Serializable]
public struct BodyData
{
    public float height;
    public float correctionRate;
}