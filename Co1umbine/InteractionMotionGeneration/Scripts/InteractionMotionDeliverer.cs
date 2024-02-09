using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Co1umbine.SHYMotionRecorder;


namespace Co1umbine.InteractionMotion
{
    /// <summary>
    /// Divide concated interaction data to each player
    /// </summary>
    public class InteractionMotionDeliverer : MonoBehaviour
    {
        [SerializeField] private MotionDataPlayerCore player1;
        [SerializeField] private PositionDataPlayerCore player1pos;
        [SerializeField] private MotionDataPlayerCore player2;
        [SerializeField] private PositionDataPlayerCore player2pos;

        [SerializeField, Tooltip("スラッシュで終わる形で")]
        private string _recordedDirectory;

        [SerializeField, Tooltip("拡張子も")]
        private string _recordedFileName;

        private List<HumanoidPoses.SerializeHumanoidPose> poseSeries1;
        private List<HumanoidPoses.SerializeHumanoidPose> poseSeries2;

        [ContextMenu("Deliver")]
        void Start()
        {
            Initiate();
        }

        private void Initiate()
        {
            if (string.IsNullOrEmpty(_recordedDirectory))
            {
                _recordedDirectory = Application.streamingAssetsPath + "/";
            }

            string motionCSVPath = _recordedDirectory + _recordedFileName;
            LoadCSVData(motionCSVPath);

            Deliver();
        }

        public void SetMotion(string directory, string fileName)
        {
            _recordedDirectory = directory;
            _recordedFileName = fileName;
            Initiate();
        }


        private void Deliver()
        {
            player1?.SetMotion(poseSeries1);
            player1pos?.SetMotion(poseSeries1);
            player2?.SetMotion(poseSeries2);
            player2pos?.SetMotion(poseSeries2);
        }

        private void LoadCSVData(string motionDataPath)
        {
            //ファイルが存在しなければ終了
            if (!File.Exists(motionDataPath))
            {
                return;
            }

            FileStream fs = null;
            StreamReader sr = null;

            poseSeries1 = new List<HumanoidPoses.SerializeHumanoidPose>();
            poseSeries2 = new List<HumanoidPoses.SerializeHumanoidPose>();

            //ファイル読み込み
            try
            {
                fs = new FileStream(motionDataPath, FileMode.Open);
                sr = new StreamReader(fs);

                while (sr.Peek() > -1)
                {
                    string line = sr.ReadLine();
                    var seriHumanPose1 = new HumanoidPoses.SerializeHumanoidPose();
                    var seriHumanPose2 = new HumanoidPoses.SerializeHumanoidPose();
                    if (line != "")
                    {
                        string[] dataString = line.Split(',');
                        if (dataString.Length < HumanoidPoses.poseSize * 2)
                        {
                            Debug.LogError("[InteractionMotionDeliverer] Invalid line length: " + dataString.Length);
                            break;
                        }
                        seriHumanPose1.DeserializeCSV(string.Join(',', dataString[0..HumanoidPoses.poseSize]));
                        seriHumanPose2.DeserializeCSV(string.Join(',', dataString[HumanoidPoses.poseSize..(HumanoidPoses.poseSize*2)]));

                        poseSeries1.Add(seriHumanPose1);
                        poseSeries2.Add(seriHumanPose2);
                    }
                }
                sr.Close();
                fs.Close();
                sr = null;
                fs = null;
            }
            catch (System.Exception e)
            {
                Debug.LogError("ファイル読み込み失敗！" + e.Message + e.StackTrace);
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
