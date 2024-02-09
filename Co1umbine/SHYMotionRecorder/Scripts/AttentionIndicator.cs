using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Co1umbine.SHYMotionRecorder
{
    public class AttentionIndicator : MotionDataPlayerCSV
    {
        private Dictionary<HumanBodyBones, Transform> attentionIndicators;

        // Start is called before the first frame update
        void Start()
        {
        }

        private void Initiate()
        {
            if (!string.IsNullOrEmpty(_recordedDirectory))
            {
                string motionCSVPath = _recordedDirectory + _recordedFileName;
                LoadCSVData(motionCSVPath);

                if (attentionIndicators == null)
                    attentionIndicators = new Dictionary<HumanBodyBones, Transform>();
            }

        }

        private void LoadCSVData(string motionDataPath)
        {
            // Exit if file does not exist
            if (!System.IO.File.Exists(motionDataPath))
            {
                return;
            }

            RecordedMotionData = ScriptableObject.CreateInstance<HumanoidPoses>();

            FileStream fs = null;
            StreamReader sr = null;

            // File loading
            try
            {
                fs = new FileStream(motionDataPath, FileMode.Open);
                sr = new StreamReader(fs);

                while (sr.Peek() > -1)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');

                    if (values.Length < 2)
                    {
                        continue;
                    }

                    HumanBodyBones bone = (HumanBodyBones)System.Enum.Parse(typeof(HumanBodyBones), values[0]);
                    Vector3 position = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));

                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }
}