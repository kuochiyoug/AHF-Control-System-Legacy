using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderPositionEstimation : MonoBehaviour
{
    public Transform trackingHeadTransform;

    public Transform rightShoulderResult;
    public Transform leftShoulderResult;

    public float heightOfBody;
    public float correctionRate = 1f;

    private float heightDiffRateEyeToShoulder = 0.1254f;
    private float oneSideShoulderWidthRate = 0.11446f;
    private float depthDiffRateEyeToShoulder = 0.06279f;
    private float armRate = 0.44f;

    public float hatsukiArmLength = 0.622f;

    public float armMappingRate { get { return hatsukiArmLength / (heightOfBody * armRate) * correctionRate; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rightShoulderResult.position
            = trackingHeadTransform.position
            + trackingHeadTransform.right * heightOfBody * oneSideShoulderWidthRate
            + trackingHeadTransform.up * heightOfBody * -heightDiffRateEyeToShoulder
            + trackingHeadTransform.forward * heightOfBody * -depthDiffRateEyeToShoulder;

        leftShoulderResult.position
            = trackingHeadTransform.position
            + trackingHeadTransform.right * heightOfBody * -oneSideShoulderWidthRate
            + trackingHeadTransform.up * heightOfBody * -heightDiffRateEyeToShoulder
            + trackingHeadTransform.forward * heightOfBody * -depthDiffRateEyeToShoulder;
    }
}
