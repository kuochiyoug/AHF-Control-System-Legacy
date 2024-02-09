using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDegreeCalculate : MonoBehaviour
{
    [SerializeField]
    private Transform[] LeftFingerCenterObjects = new Transform[5];

    [SerializeField]
    private Transform[] RightFingerCenterObjects = new Transform[5];

    [SerializeField]
    private Transform[] LeftFingerObjects = new Transform[5];

    [SerializeField]
    private Transform[] RightFingerObjects = new Transform[5];

    [SerializeField]
    private float[] LeftFingerDegreeDiffs = new float[5];

    [SerializeField]
    private float[] RightFingerDegreeDiffs = new float[5];

    void Update()
    {
        for(int i=0; i<5; i++)
        {
            LeftFingerDegreeDiffs[i] = CalculateDegreeDiff(LeftFingerObjects[i], LeftFingerCenterObjects[i]);
            RightFingerDegreeDiffs[i] = CalculateDegreeDiff(RightFingerObjects[i], RightFingerCenterObjects[i]);
        }
    }

    private float CalculateDegreeDiff (Transform ParentObject, Transform ChildObject)
    {
        Quaternion LocalRotation = Quaternion.Inverse(ParentObject.rotation) * ChildObject.rotation;
        float diff = LocalRotation.eulerAngles.x;

        // Alignment eulerAngles
        if (LocalRotation.eulerAngles.y > 90.0f)
        {
            if (LocalRotation.eulerAngles.x < 180.0f) diff = 180.0f - LocalRotation.eulerAngles.x;
            else diff = 540.0f - LocalRotation.eulerAngles.x;
        }

        // Clamp 0~360 to -180~180
        if (diff > 180.0f) diff -= 360.0f;

        return diff;
    }
}
