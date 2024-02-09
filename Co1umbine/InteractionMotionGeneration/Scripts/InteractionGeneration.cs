using Co1umbine.SHYMotionRecorder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Co1umbine.InteractionMotion.Generation
{
    [DefaultExecutionOrder(11000)]
    public class InteractionGeneration : MonoBehaviour
    {
        public Animator sourceLeaderAnimator;
        public Animator sourceFollowerAnimator;
        public MotionDataPlayerCore motionPlayer;
        public PositionDataPlayerCore positionPlayer;
        public MotionDataPlayerCore leaderMotionPlayer;
        public PositionDataPlayerCore leaderPositionPlayer;

        public float TargetFPS = 30;

        public readonly int rigSize = 467;

        public float waitForStartSec = 3f;

        public int runUpStep = 20;

        [SerializeField] private string modelPath = "Co1umbine/InteractionMotionGeneration/ONNX/lstm_setup9_runupfix10000.onnx";
        [SerializeField] private string meanPath;
        [SerializeField] private string stdPath;

        public bool generate = false;

        private HumanPoseHandler leaderPoseHandler;
        private HumanPoseHandler followerPoseHandler;

        private float[] mean;
        private float[] std;

        private LSTM lstm;

        private float lastRecordTime = 0;

        private bool isWaiting = false;

        private float startTime = 0;

        [SerializeField] private bool positionFix = false;
        private Vector3? fixedPosition;

        
        public bool debug = false;
        private GUIStyle style;

        public float[] hidden;

        private void Start()
        {
            style = new GUIStyle();
            var styleState = new GUIStyleState();
            style.fontSize = 20;
            styleState.textColor = Color.black;
            style.normal = styleState;
            Initiate();
        }

        private void Initiate()
        {
            lstm = LSTM.Factory.CreateInteractionLSTM(modelPath);

            leaderPoseHandler = new HumanPoseHandler(sourceLeaderAnimator.avatar, sourceLeaderAnimator.transform);
            followerPoseHandler = new HumanPoseHandler(sourceFollowerAnimator.avatar, sourceFollowerAnimator.transform);

            mean = Load1DimensionCSV(meanPath);
            std = Load1DimensionCSV(stdPath);

            var humanPose = new HumanPose();
            followerPoseHandler.GetHumanPose(ref humanPose);
            fixedPosition = humanPose.bodyPosition;
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
                if (!generate && !isWaiting)
                {
                    isWaiting = true;
                    startTime = Time.time;
                }
                else
                {
                    generate = false;
                    isWaiting = false;
                    // reset state
                    lstm.ResetContext();
                    runningStep = 0;
                    fixedPosition = null;
                }
            }

            if (isWaiting)
            {
                var elapsed = Time.time - startTime;
                if (elapsed >= waitForStartSec)
                {
                    generate = true;
                    isWaiting = false;
                }
            }

            if (generate)
            {
                var elapsed = Time.time - lastRecordTime;

                if (elapsed < 1 / TargetFPS)
                {
                    return;
                }

                RunGeneration();
                lastRecordTime = Time.time;
                hidden = lstm.hidden;
            }
        }

        private HumanoidPoses.SerializeHumanoidPose lastLeaderPose;
        private HumanoidPoses.SerializeHumanoidPose lastFollowerPose;
        private int runningStep = 0;
        //private float[] lastOutput;
        private void RunGeneration()
        {
            float[] inputData = CorrectData();

            inputData = LSTM.Preprocess(inputData, mean, std);

            if (debug)
            {
                inputData.Where((x, i) => (Mathf.Abs(x)) > 2).Select((x, i) => (x, i)).ToList().ForEach(xi => Debug.Log($"index: {xi.i} value: {xi.x}"));
            }

            float[] outputData = lstm.Run(inputData);

            outputData = LSTM.Postprocess(outputData, mean, std);

            if (runningStep > runUpStep) { 
                ApplyPose(outputData[rigSize..(rigSize + rigSize)]);
                LeaderApplyPose(outputData[0..rigSize]);
            }

            runningStep++;
        }

        private float[] CorrectData()
        {
            var leaderPose = GetPose(sourceLeaderAnimator, leaderPoseHandler);
            if (lastLeaderPose == null)
                lastLeaderPose = leaderPose;

            string leaderPoseCsv = leaderPose.SerializeCSV(lastLeaderPose);
            //if(debug)
            //    Debug.Log(leaderPoseCsv);
            
            lastLeaderPose = leaderPose;


            var followerPose = GetPose(sourceFollowerAnimator, followerPoseHandler);
            if (lastFollowerPose == null)
                lastFollowerPose = followerPose;

            if (positionFix)
            {
                fixedPosition ??= followerPose.BodyPosition;
                followerPose.BodyPosition = fixedPosition.Value;
            }
            string followerPoseCsv = followerPose.SerializeCSV(lastFollowerPose);

            lastFollowerPose = followerPose;

            Vector3 relativePosition = leaderPose.BodyPosition - followerPose.BodyPosition;
            Quaternion relativeRotation = leaderPose.BodyRotation * Quaternion.Inverse(followerPose.BodyRotation);


            string inputDataString = leaderPoseCsv + "," + followerPoseCsv + ","
                + relativePosition.x + "," + relativePosition.y + "," + relativePosition.z + ","
                + relativeRotation.x + "," + relativeRotation.y + "," + relativeRotation.z + "," + relativeRotation.w;

            float[] result = inputDataString.Split(',').Select(x => float.Parse(x)).ToArray();

            //if (lastOutput != null)
            //    Array.Copy(lastOutput, rigSize, result, rigSize, rigSize);

            Debug.Assert(result.Length == 941, $"Input data preprocess error. Data length was {result.Length} while expected 941.");

            return result;
        }
        private void ApplyPose(float[] data)
        {
            var serializedPose = new HumanoidPoses.SerializeHumanoidPose();
            serializedPose.Deserialize(data);
            //string dataString = data.Select(x => x.ToString()).Aggregate((x, y) => x + "," + y);
            //serializedPose.DeserializeCSV(dataString);

            if (positionFix)
                serializedPose.BodyPosition = fixedPosition.Value;

            motionPlayer?.DirectSetMotion(serializedPose);
            positionPlayer?.DirectSetMotion(serializedPose);
        }

        private void LeaderApplyPose(float[] data)
        {
            var serializedPose = new HumanoidPoses.SerializeHumanoidPose();
            serializedPose.Deserialize(data);
            //string dataString = data.Select(x => x.ToString()).Aggregate((x, y) => x + "," + y);
            //serializedPose.DeserializeCSV(dataString);

            //if(positionFix)
            //    serializedPose.BodyPosition = fixedPosition.Value;

            leaderMotionPlayer?.DirectSetMotion(serializedPose);
            leaderPositionPlayer?.DirectSetMotion(serializedPose);
        }

        private HumanoidPoses.SerializeHumanoidPose GetPose(Animator animator, HumanPoseHandler poseHandler)
        {
            HumanPose _currentPose = default(HumanPose);
            poseHandler.GetHumanPose(ref _currentPose);
            //poseHandler.GetHumanPose(ref _currentPose);

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
            lstm?.Dispose();
        }

        private void OnGUI()
        {
            if (generate)
            {

                GUI.Label(new Rect(10, 10, 100, 20), "Generating...", style);
            }
            else if (isWaiting)
            {
                var elapsed = Time.time - startTime;
                GUI.Label(new Rect(10, 10, 100, 20), "Waiting..." + Mathf.Abs(waitForStartSec - elapsed), style);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            var pose = new HumanPose();
            leaderPoseHandler?.GetHumanPose(ref pose);
            Gizmos.DrawSphere(pose.bodyPosition, 0.07f);

            Gizmos.color = Color.green;
            if (lastLeaderPose != null)
            {
                Gizmos.DrawSphere(lastLeaderPose.BodyPosition, 0.05f);
            }
        }
    }
}