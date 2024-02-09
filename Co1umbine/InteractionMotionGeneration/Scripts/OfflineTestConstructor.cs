using Co1umbine.SHYMotionRecorder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Co1umbine.InteractionMotion
{
    public class OfflineTestConstructor : MonoBehaviour
    {
        [Header("Ground Truth")]

        public string targetDirectory;

        public List<string> targetCsvFileNames;

        public List<InteractionMotionDeliverer> deliverer;

        [Header("Generated Motion")]
        public string testDirectory;

        public List<string> testCsvFileNames;

        public MotionDataPlayerCSV generatedMotionPlayer;
        public PositionDataPlayerCSV generatedPositionPlayer;

        [Header("SetUp")]
        public string targetFileNamePattern;
        public string testFileNamePattern;
        private string lastMatchedTarget;
        private string lastMatchedTest;

        // Start is called before the first frame update
        void Start()
        {
            Initiate();
            SetUp();
        }

        private void Initiate()
        {
            // Get all CSV fileNames in the directory
            if (targetDirectory != "")
            {
                var _targetFileNames = System.IO.Directory.GetFiles(targetDirectory, "*.csv");
                foreach (var n in _targetFileNames)
                {
                    targetCsvFileNames.Add(n.Split('/')[^1]);
                }
            }

            // Get all CSV fileNames in the directory
            if (testDirectory != "")
            {
                var _testFileNames = System.IO.Directory.GetFiles(testDirectory, "*.csv");
                foreach (var n in _testFileNames)
                {
                    testCsvFileNames.Add(n.Split('/')[^1]);
                }
            }
        }

        public void SetUp()
        {


            // Get FileNames matching the keyword
            var matchedTarget = targetCsvFileNames.Find(f => Regex.IsMatch(f, targetFileNamePattern));
            var matchedTest = testCsvFileNames.Find(f => Regex.IsMatch(f, testFileNamePattern));

            if (matchedTarget == "")
            {
                lastMatchedTarget = "";
                return;
            }
            if (matchedTest == "") {
                lastMatchedTest = "";
                return;
            }

            // Set the file name to the deliverer
            if (lastMatchedTarget != matchedTarget)
            {
                deliverer.ForEach(d => d.SetMotion(targetDirectory, matchedTarget));
                lastMatchedTarget = matchedTarget;
            }

            // Set the file name to the generated motion
            if (lastMatchedTest != matchedTest)
            {
                generatedMotionPlayer.LoadMotion(testDirectory, matchedTest);
                generatedPositionPlayer.LoadMotion(testDirectory, matchedTest);
                lastMatchedTest = matchedTest;
            }
        }

        // Inspector Editor Extention Inner Class
#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(OfflineTestConstructor))]
        public class DemoOfflineConstructorEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                var offlineTestConstructor = target as OfflineTestConstructor;

                if (GUILayout.Button("Set Up"))
                {
                    offlineTestConstructor.SetUp();
                }


                EditorGUI.BeginDisabledGroup(true);

                // readonly text field
                UnityEditor.EditorGUILayout.TextField("Last Matched Target", offlineTestConstructor.lastMatchedTarget);
                

                // readonly text field
                UnityEditor.EditorGUILayout.TextField("Last Matched Test", offlineTestConstructor.lastMatchedTest);
                

                EditorGUI.EndDisabledGroup();
            }
        }
#endif
    }
}