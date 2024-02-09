using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReference : MonoBehaviour
{
    public List<string> loadSceneNames = new List<string>();

    private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var n in loadSceneNames)
        {
            var tmp = SceneManager.GetSceneByName(n);
            if (tmp.IsValid())
            {
                scenes.Add(n, tmp);
            }
        }
    }

    public Scene GetScene(string sceneName)
    {
        return scenes[sceneName];
    }

}
