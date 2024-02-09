using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKControlAPI : MonoBehaviour
{
    [SerializeField]
    private GetAngleFromSelf AngleStart;

    [SerializeField]
    private BioIK.BioIK bioIK;

    public bool IKControl
    {
        get{ return AngleStart.IKControlActivation;}
        set{ AngleStart.IKControlActivation = value;}
    }

    public float BioIKSmoothing
    {
        get { return bioIK.Smoothing; }
        set { bioIK.Smoothing = value; }
    }

    public float BioIKMaxVelocity
    {
        get { return bioIK.MaximumVelocity; }
        set { bioIK.MaximumVelocity = value; }
    }

    public float BioIKMaxAcceleration
    {
        get { return bioIK.MaximumAcceleration; }
        set { bioIK.MaximumAcceleration = value; }
    }

}
