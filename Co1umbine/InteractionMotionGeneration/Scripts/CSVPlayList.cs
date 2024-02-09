using Co1umbine.SHYMotionRecorder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Co1umbine
{
    public class CSVPlayList : MonoBehaviour
    {
        [System.Serializable]
        public class NameTemplate
        {
            [SerializeReference] public MotionDataPlayerCSV player;
            public string startWith;
            public List<string> names;
        }

        [SerializeField]
        private string _targetDirectory;

        [SerializeField]
        private List<NameTemplate> players;

        [SerializeField]
        private int index = 0;

        private void Start()
        {
            LoadMotion();
            if(players.Count > 0 && players[0].names.Count > 0)
            SetMotion(0);
        }

        [ContextMenu("Load Motion")]
        public void LoadMotion()
        {
            // Get all CSV fileNames in the directory
            var _fileNames = System.IO.Directory.GetFiles(_targetDirectory, "*.csv");
            foreach(var n in _fileNames)
            {
                foreach(var p in players)
                {
                    if (n.Split('/')[^1].StartsWith(p.startWith))
                    {
                        p.names.Add(n.Split('/')[^1]);
                    }
                }
            }
        }

        public void SetMotion(int index)
        {
            if (index < 0)
            {
                return;
            }

            foreach(var p in players)
            {
                if (index < p.names.Count)
                {
                    p.player.LoadMotion(_targetDirectory, p.names[index]);
                }
            }
            foreach(var p in players)
            {
                p.player.PlayMotion();
            }
        }

#if UNITY_EDITOR
        // Editor extension
        [UnityEditor.CustomEditor(typeof(CSVPlayList))]
        public class CSVPlayListEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                var list = target as CSVPlayList;

                if (GUILayout.Button("Load Motion"))
                {
                    list.LoadMotion();
                }

                // add space
                GUILayout.Space(20);

                // Show index
                GUILayout.Label("Index: " + list.index);

                if (GUILayout.Button("Set Prev Motion"))
                {
                    list.index--;
                    list.SetMotion(list.index);
                }
                if (GUILayout.Button("Set Next Motion"))
                {
                    list.index++;
                    list.SetMotion(list.index);
                }
            }
        }
        #endif
    }
}