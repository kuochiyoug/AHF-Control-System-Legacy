using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidJointPositionCalculator : MonoBehaviour
{
    public GameObject TargetMidPoint;

    public Transform RootJoint;
    public Transform MidJoint;
    public Transform EndJoint;
    public Transform Pole;


    public float TotalLength { get; private set; }
    public float UpperLength { get; private set; }
    public float LowerLength { get; private set; }


    //public bool ShowIndicator = false;
    private bool ready;


    private void Awake()
    {
        if (RootJoint == null | MidJoint == null | EndJoint == null | Pole == null)
            return;
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready)
            return;

        Vector3 polevector = Pole.position - CalculateProjectionOn(RootJoint, EndJoint, Pole); //OrigPoleVector
        Vector3 NewMidPos = CalculateMidPointPosition(RootJoint, MidJoint, EndJoint, polevector);

        TargetMidPoint.transform.position = NewMidPos;
    }


    Vector3 CalculateProjectionOn(Transform Root, Transform End, Transform ProjectionObj)
    {
        // Calculate Projection position on RE line
        Vector3 LimbVector = End.position - Root.position;
        Vector3 ProjectionObjVector = ProjectionObj.position - Root.position;
        Vector3 ProjectionObjProjPt = Root.position + Vector3.Dot(ProjectionObjVector, LimbVector) / Vector3.Dot(LimbVector, LimbVector) * LimbVector;
        return ProjectionObjProjPt;
    }


    Vector3 CalculateMidPointPosition(Transform Root, Transform Mid, Transform End,  Vector3 PoleVector)
    {
        LowerLength = (EndJoint.position - MidJoint.position).magnitude;
        UpperLength = (MidJoint.position - RootJoint.position).magnitude;
        TotalLength = LowerLength + UpperLength;

        Vector3 LimbVector = End.position - Root.position; //RE
        float LengthRatio = UpperLength / TotalLength;
        Vector3 ProjNewMp = Root.position + LengthRatio * LimbVector;
        float cosine = (ProjNewMp - Root.position).magnitude / UpperLength;
        float sine = Mathf.Sqrt(1 - Mathf.Pow(cosine, 2));
        float magnitude = UpperLength * sine;
        Vector3 MidPointPosition = ProjNewMp + magnitude * PoleVector.normalized;

        return MidPointPosition;
    }


}