using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FacialExAnimationEventHolder : MonoBehaviour
{
    public bool active;
    private FacialControlAPI face;

    private void Start()
    {


        var scene = SceneManager.GetSceneByName("ProjectionFace");
        if (scene.IsValid())
        {
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                face = rootGameObject.GetComponent<FacialControlAPI>();
                if (face != null)
                {
                    Debug.Log("Found FacialAPI");
                    break;
                }
            }
        }
    }

    public void FacialEvent(string animationName)
    {
        if (!active) return;

        face?.PlayAnimation(animationName);
        Debug.Log("[FacialExAnimationEvnetHolder] gat animationName " + animationName);
    }

}
