using Co1umbine.SHYMotionRecorder;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Co1umbine.InteractionMotion.Generation
{
    public class InteractionGenerationWebSocket : MonoBehaviour
    {
        public Animator sourceLeaderAnimator;
        public Animator sourceFollowerAnimator;
        public MotionDataPlayerCore motionPlayer;
        public PositionDataPlayerCore positionPlayer;

        public float TargetFPS = 30;

        public readonly int rigSize = 467;

        public int runUpStep = 20;

        private HumanPoseHandler leaderPoseHandler;
        private HumanPoseHandler followerPoseHandler;

        [SerializeField] private string meanPath;
        [SerializeField] private string stdPath;

        private float[] mean;
        private float[] std;

        private WebSocketInference lstmClient;

        public bool generate = false;

        private float lastRecordTime = 0;
        private float lastUpdateTime = 0;

        private string address = "localhost";
        private string port = "8765";

        private void Start()
        {
            Initiate();
        }

        private void Initiate()
        {
            lstmClient = new WebSocketInference(address, port);

            leaderPoseHandler = new HumanPoseHandler(sourceLeaderAnimator.avatar, sourceLeaderAnimator.transform);
            followerPoseHandler = new HumanPoseHandler(sourceFollowerAnimator.avatar, sourceFollowerAnimator.transform);

            mean = Load1DimensionCSV(meanPath);
            std = Load1DimensionCSV(stdPath);
        }

        private float[] Load1DimensionCSV(string path)
        {
            List<float> result = new List<float>();

            FileStream fs = null;
            StreamReader sr = null;

            // file loading
            try
            {
                fs = new FileStream(path, FileMode.Open);
                sr = new StreamReader(fs);

                while (sr.Peek() > -1)
                {
                    string value = sr.ReadLine();
                    float m = float.Parse(value);
                    result.Add(m);
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError("[LSTM] Failed to load file in preprocess.\n" + e.Message + e.StackTrace);
            }
            finally
            {
                sr?.Close();
                fs?.Close();
            }

            return result.ToArray();
        }

        private void LateUpdate()
        {
            if(Input.GetKeyDown(KeyCode.R)) 
            {
                if (generate)
                {
                    lstmClient.ResetContext();
                    runningStep = 0;

                }
                generate = !generate; 
            }

            if (generate)
            {
                if(lstmClient.lastRecieveTime >= lastUpdateTime)
                {
                    HandleLatestData();
                    lastUpdateTime = lstmClient.lastRecieveTime;
                }

                var elapsed = Time.time - lastRecordTime;

                if (elapsed < 1 / TargetFPS)
                {
                    return;
                }

                RunGeneration();
                lastRecordTime = Time.time;
            }
        }

        private HumanoidPoses.SerializeHumanoidPose lastLeaderPose;
        private HumanoidPoses.SerializeHumanoidPose lastFollowerPose;
        private int runningStep = 0;
        private void RunGeneration()
        {
            float[] inputData = CorrectData();

            inputData = LSTM.Preprocess(inputData, mean, std);

            lstmClient.Run(inputData);
        }


        private float[] CorrectData()
        {
            var leaderPose = GetPose(sourceLeaderAnimator, leaderPoseHandler);
            if (lastLeaderPose == null)
            {
                lastLeaderPose = leaderPose;
            }
            string leaderPoseCsv = leaderPose.SerializeCSV(lastLeaderPose);
            
            lastLeaderPose = leaderPose;


            var followerPose = GetPose(sourceFollowerAnimator, followerPoseHandler);
            if (lastFollowerPose == null)
            {
                lastFollowerPose = followerPose;
            }
            string followerPoseCsv = followerPose.SerializeCSV(lastFollowerPose);

            lastFollowerPose = followerPose;

            Vector3 relativePosition = leaderPose.BodyPosition - followerPose.BodyPosition;
            Quaternion relativeRotation = leaderPose.BodyRotation * Quaternion.Inverse(followerPose.BodyRotation);


            string inputDataString = leaderPoseCsv + "," + followerPoseCsv + ","
                + relativePosition.x + "," + relativePosition.y + "," + relativePosition.z + ","
                + relativeRotation.x + "," + relativeRotation.y + "," + relativeRotation.z + "," + relativeRotation.w;

            float[] result = inputDataString.Split(',').Select(x => float.Parse(x)).ToArray();

            Debug.Assert(result.Length == 941, $"Input data preprocess error. Data length was {result.Length} while expected 941.");

            return result;
        }

        private void HandleLatestData()
        {
            var outputData = lstmClient.GetResult();
            if(outputData == null)
            {
                return;
            }
            outputData = LSTM.Postprocess(outputData, mean, std);

            Debug.Log(outputData.Length);
            Debug.Log(string.Join(", ", outputData));

            if (runningStep > runUpStep)
                ApplyPose(outputData[rigSize..(rigSize + rigSize)]);

            runningStep++;
        }

        private void ApplyPose(float[] data)
        {
            var serializedPose = new HumanoidPoses.SerializeHumanoidPose();
            //serializedPose.Deserialize(data);
            string dataString = data.Select(x => x.ToString()).Aggregate((x, y) => x + "," + y);
            serializedPose.DeserializeCSV(dataString);

            motionPlayer?.DirectSetMotion(serializedPose);
            positionPlayer?.DirectSetMotion(serializedPose);
        }

        private HumanoidPoses.SerializeHumanoidPose GetPose(Animator animator, HumanPoseHandler poseHandler)
        {
            HumanPose _currentPose = default(HumanPose);
            poseHandler.GetHumanPose(ref _currentPose);

            var serializedPose = new HumanoidPoses.SerializeHumanoidPose();


            serializedPose.BodyPosition = _currentPose.bodyPosition;
            serializedPose.BodyRotation = _currentPose.bodyRotation;
            serializedPose.Muscles = new float[_currentPose.muscles.Length];
            for (int i = 0; i < serializedPose.Muscles.Length; i++)
            {
                serializedPose.Muscles[i] = _currentPose.muscles[i];
            }

            MotionDataRecorder.SetHumanBoneTransformToHumanoidPoses(animator, ref serializedPose);

            return serializedPose;
        }

        private void OnDestroy()
        {
            lstmClient?.Dispose();
        }
    }
}