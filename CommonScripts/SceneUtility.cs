using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;

namespace CutieroidUtility
{
    public static class SceneUtility
    {
        public static  GameObject GetGameObjectInScene(Scene s, string GameObjectName)
        {
            GameObject[] gameObjects = s.GetRootGameObjects();
            Debug.Log("[buildtest] In " + s.name + " gameObjects count " + gameObjects.Count());
            Debug.Log("[buildtest] GameObjectName " + GameObjectName);
            foreach (GameObject game_obj in gameObjects)
            {
                if (game_obj.name == GameObjectName)
                {
                    Debug.Log("[buildtest] game_obj.name " + game_obj.name);
                    return game_obj;
                }
            }
            return null;
        }

        public static void DisableAllEventSystemsInScene(Scene s)
        {
            //var eventSystemsGameObjects = GameObject.FindObjectsOfType<EventSystem>();
            GameObject[] gameObjects = s.GetRootGameObjects();
            foreach(GameObject gameObject in gameObjects)
            {
                var eventsystems = gameObject.GetComponents<EventSystem>();
                foreach (var ES in eventsystems)
                {
                    ES.enabled = false;
                    Debug.Log("[buildtest] Disable EventSystem in Scene " + s.name + " on " + gameObject.name + "!");
                }
            }
        }

        public static void DisableAllAudioListnerInScene(Scene s)
        {
            //var eventSystemsGameObjects = GameObject.FindObjectsOfType<EventSystem>();
            GameObject[] gameObjects = s.GetRootGameObjects();
            foreach (GameObject gameObject in gameObjects)
            {
                var audiolistners = gameObject.GetComponents<AudioListener>();
                foreach (var AL in audiolistners)
                {
                    AL.enabled = false;
                    Debug.Log("[buildtest] Disable AudioListner in Scene " + s.name + " on " + gameObject.name + "!");
                }
            }
        }

        public static List<T> GetTypeComponentsInScene<T>(Scene s)
        {
            GameObject[] gameObjects = s.GetRootGameObjects();
            return GetTypeComponentsInGameObjectArray<T>(gameObjects);
        }

        public static List<T> GetTypeComponentsInGameObjectArray<T>(GameObject[] gameObjects)
        {
            List<T> typeComponentsList = new List<T>();
            foreach (GameObject gameObject in gameObjects)
            {
                var components = gameObject.GetComponents<T>();
                foreach (var component in components)
                {
                    if (!typeComponentsList.Contains(component))
                        typeComponentsList.Add(component);
                }

                components = gameObject.GetComponentsInChildren<T>();
                foreach (var component in components)
                {
                    if (!typeComponentsList.Contains(component))
                        typeComponentsList.Add(component);
                }
            }
            return typeComponentsList;
        }

    }

    public class DontDestroyOnLoadCollector
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                var ddol = CollectDontDestroyOnLoad();
                var separator = ", ";
                var s = string.Join(separator, ddol.Where(go => go != null).Select(go => go.name));
                Debug.Log($"DDOL_{scene.name} : {s}");
            };
        }

        public static GameObject[] CollectDontDestroyOnLoad()
        {
            var go = new GameObject(string.Empty);
            Object.DontDestroyOnLoad(go);
            var ddol = go.scene.GetRootGameObjects();
            Object.Destroy(go);
            return ddol.Where(o => o != null && !string.Equals(o.name, string.Empty)).ToArray();
        }
    }
}