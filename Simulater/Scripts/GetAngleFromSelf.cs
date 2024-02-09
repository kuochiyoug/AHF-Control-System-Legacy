using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// Read joint angle from BIOIK, then send value to AngleSetter
/// </summary>
public class GetAngleFromSelf : MonoBehaviour
{
    [SerializeField] private BioIK.BioIK bioIK;
    [SerializeField] private bool useIKControl = false;

    private AngleSetter angleSetter;
    private MotorMsgs motorMsgs= new MotorMsgs();
    private MotionPlayer motion;

    public bool IKControlActivation
    {
        get { return useIKControl; }
        set { useIKControl = value; }
    }

    private void Start()
    {
        angleSetter = GetComponent<AngleSetter>();
        
        Scene scene = SceneManager.GetSceneByName("MotorControlPanel");
        if (scene.IsValid())
        {
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                motion = rootGameObject.GetComponent<MotionPlayer>();
                if (motion != null)
                {
                    //Debug.Log("Found MotionPlayer");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("MotorControlPanel is Invalid");
        }
    }

    private void Update()
    {
        if (useIKControl)
        {
            if (motion != null && motion.MotionOnGoing)
            {
                return;
            }
            motorMsgs.s1 = (float)bioIK.FindSegment("neck.y").Joint.Z.CurrentValue;
            motorMsgs.s2 = (float)bioIK.FindSegment("neck.z").Joint.Y.CurrentValue;
            motorMsgs.s3 = (float)bioIK.FindSegment("Head").Joint.X.CurrentValue;

            motorMsgs.s20 = (float)bioIK.FindSegment("Right shoulder").Joint.Z.CurrentValue;
            motorMsgs.s21 = (float)bioIK.FindSegment("arm.R.y").Joint.X.CurrentValue;
            motorMsgs.s22 = (float)bioIK.FindSegment("uparm.R.y").Joint.Z.CurrentValue;
            motorMsgs.s23 = (float)bioIK.FindSegment("elbow.R.y").Joint.X.CurrentValue;
            motorMsgs.s24 = (float)bioIK.FindSegment("forearm.R.y").Joint.Z.CurrentValue;
            motorMsgs.s25 = (float)bioIK.FindSegment("hand.R").Joint.X.CurrentValue;

            motorMsgs.s30 = (float)bioIK.FindSegment("Left shoulder").Joint.Z.CurrentValue;
            motorMsgs.s31 = (float)bioIK.FindSegment("arm.L.y").Joint.X.CurrentValue;
            motorMsgs.s32 = (float)bioIK.FindSegment("uparm.L.y").Joint.Z.CurrentValue;
            motorMsgs.s33 = (float)bioIK.FindSegment("elbow.L.y").Joint.X.CurrentValue;
            motorMsgs.s34 = (float)bioIK.FindSegment("forearm.L.y").Joint.Z.CurrentValue;
            motorMsgs.s35 = (float)bioIK.FindSegment("hand.L").Joint.X.CurrentValue;

            motorMsgs.exp = "";

            angleSetter.FromMotorMsgs(motorMsgs);
        }
        else
        {
            //if (angleSetter.prevMsgs != null)
            //{
            //    var motorMsg = angleSetter.prevMsgs;
            //    bioIK.FindSegment("neck.y").Joint.Z.TargetValue = motorMsg.s1;
            //    bioIK.FindSegment("neck.z").Joint.Y.TargetValue = motorMsg.s2;
            //    bioIK.FindSegment("Head").Joint.X.TargetValue = motorMsg.s3;

            //    bioIK.FindSegment("Right shoulder").Joint.Z.TargetValue = motorMsg.s20;
            //    bioIK.FindSegment("arm.R.y").Joint.X.TargetValue = motorMsg.s21;
            //    bioIK.FindSegment("uparm.R.y").Joint.Z.TargetValue = motorMsg.s22;
            //    bioIK.FindSegment("elbow.R.y").Joint.X.TargetValue = motorMsg.s23;
            //    bioIK.FindSegment("forearm.R.y").Joint.Z.TargetValue = motorMsg.s24;
            //    bioIK.FindSegment("hand.R").Joint.X.TargetValue = motorMsg.s25;

            //    bioIK.FindSegment("Left shoulder").Joint.Z.TargetValue = motorMsg.s30;
            //    bioIK.FindSegment("arm.L.y").Joint.X.TargetValue = motorMsg.s31;
            //    bioIK.FindSegment("uparm.L.y").Joint.Z.TargetValue = motorMsg.s32;
            //    bioIK.FindSegment("elbow.L.y").Joint.X.TargetValue = motorMsg.s33;
            //    bioIK.FindSegment("forearm.L.y").Joint.Z.TargetValue = motorMsg.s34;
            //    bioIK.FindSegment("hand.L").Joint.X.TargetValue = motorMsg.s35;
            //}
        }
    }
}
