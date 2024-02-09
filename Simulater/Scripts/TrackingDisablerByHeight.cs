using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingDisablerByHeight : MonoBehaviour
{
    public Transform headTrackerTransform;
    public List<HandTrackCalibrator> handTrack;
    public List<RotateSimilarToCam> rotateTrack;
    public List<WeaknessPositionHolder> weaknessPositionHolders;

    public float threshold;

    [SerializeField] private bool isEnable = true;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(headTrackerTransform.position.y >= threshold)
        {
            if (!isEnable)
            {
                Debug.Log("loop start order");
                foreach (var wph in weaknessPositionHolders)
                {
                    wph.StartTracking();
                }
                foreach (var zct in handTrack)
                {
                    zct.enabled = true;
                }
                foreach(var rt in rotateTrack)
                {
                    rt.enabled = true;
                }
                isEnable = true;
            }
        }
        else
        {
            if (isEnable)
            {
                Debug.Log("loop stop order");
                foreach(var zct in handTrack)
                {
                    zct.enabled = false;
                }
                foreach (var wph in weaknessPositionHolders)
                {
                    wph.GoToWeaknessPosition();
                }
                foreach(var rt in rotateTrack)
                {
                    rt.enabled = false;
                }
                isEnable = false;
            }
        }
    }
}
