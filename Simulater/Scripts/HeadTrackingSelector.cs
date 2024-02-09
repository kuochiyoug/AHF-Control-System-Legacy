using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrackingSelector : MonoBehaviour
{
    [SerializeField] private bool useHmd;
    public Transform hmd;
    public Transform tracker;
    public Vector3 trackerRotateOffset;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (useHmd)
        {
            transform.position = hmd.position;
            transform.rotation = hmd.rotation;
        }
        else
        {
            transform.position = tracker.position;
            transform.rotation = tracker.rotation * Quaternion.Euler(trackerRotateOffset);
        }
    }
}
