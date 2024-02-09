using BioIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicConstraint : MonoBehaviour
{
    [SerializeField] Transform rootTransform;
    [SerializeField] JointValue jointValue;
    [SerializeField] ShoulderPositionEstimation estimation;
    [SerializeField, Range(0, 1)] float hatsukiArmAdjustRate = 0.741f;
    [SerializeField] float steadyConstraint = 10;
    [SerializeField] bool isRight = true;

    // Update is called once per frame
    void Update()
    {
        var downPosRate = -Mathf.Clamp((transform.position.y - rootTransform.position.y) / (estimation.hatsukiArmLength * hatsukiArmAdjustRate), -1, 1);
        var dist = Mathf.Clamp((transform.position - rootTransform.position).magnitude / (estimation.hatsukiArmLength * hatsukiArmAdjustRate), 0, 1);
        var dir = 1;
        if (isRight)
        {
            dir = -1;
        }

        jointValue.SetTargetValue(dir * Mathf.Acos(downPosRate) * Mathf.Rad2Deg);
        //if (transform.position.y < rootTransform.position.y)
        //{
        //    jointValue.SetTargetValue(dir * (-Mathf.Asin(upPosRate) * Mathf.Rad2Deg + steadyConstraint + 90 * (1-dist)));
        //}
        //else
        //{
        //    jointValue.SetTargetValue(dir * ((Mathf.Asin(upPosRate) - Mathf.PI) * Mathf.Rad2Deg + steadyConstraint + 90 * (1-dist)));
        //}
    }
}
