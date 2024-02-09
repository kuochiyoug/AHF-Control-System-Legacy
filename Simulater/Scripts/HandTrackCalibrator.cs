using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackCalibrator : MonoBehaviour
{
    public Transform handTarget;
    public Transform rootPosition;
    public Transform calibrateRootTarget;

    public float maxSpeed = 0.1f;

    [SerializeField] ShoulderPositionEstimation estimation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = MoveTowardVec(TargetPositionSet());
    }

    private Vector3 TargetPositionSet()
    {
        var result = calibrateRootTarget.position
            + (handTarget.position - rootPosition.position) * estimation.armMappingRate;

        if(result.z < calibrateRootTarget.position.z)
        {
            result = new Vector3(result.x, result.y, calibrateRootTarget.position.z);
        }
        return result;
    }

    private Vector3 MoveTowardVec(Vector3 targetVec)
    {
        return Vector3.MoveTowards(transform.position, targetVec, maxSpeed * Time.deltaTime);
    }
}
