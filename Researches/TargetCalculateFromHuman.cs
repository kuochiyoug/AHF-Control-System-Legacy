using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCalculateFromHuman : MonoBehaviour
{
    public GameObject humanoid;
    public GameObject target_indicator;

    Transform AnimationRoot;

    Transform Hips;
    Transform Spine;
    Transform Neck;
    Transform Head;
    Transform leftUpperArm;
    Transform leftLowerArm;
    Transform leftHand;
    Transform rightUpperArm;
    Transform rightLowerArm;
    Transform rightHand;
    Transform leftUpperLeg;
    Transform leftLowerLeg;
    Transform leftFoot;
    Transform leftToes;
    Transform rightUpperLeg;
    Transform rightLowerLeg;
    Transform rightFoot;
    Transform rightToes;

    Transform leftThumbDistal;
    Transform leftThumbIntermediate;
    Transform leftThumbProximal;
    Transform leftIndexDistal;
    Transform leftIndexIntermediate;
    Transform leftIndexProximal;
    Transform leftMiddleDistal;
    Transform leftMiddleIntermediate;
    Transform leftMiddleProximal;
    Transform leftRingDistal;
    Transform leftRingIntermediate;
    Transform leftRingProximal;
    Transform leftLittleDistal;
    Transform leftLittleIntermediate;
    Transform leftLittleProximal;
    Transform rightThumbDistal;
    Transform rightThumbIntermediate;
    Transform rightThumbProximal;
    Transform rightIndexDistal;
    Transform rightIndexIntermediate;
    Transform rightIndexProximal;
    Transform rightMiddleDistal;
    Transform rightMiddleIntermediate;
    Transform rightMiddleProximal;
    Transform rightRingDistal;
    Transform rightRingIntermediate;
    Transform rightRingProximal;
    Transform rightLittleDistal;
    Transform rightLittleIntermediate;
    Transform rightLittleProximal;

    Transform rightArmPole;
    Transform leftArmPole;


    GameObject targetHips_ind;
    GameObject targetSpine_ind;
    GameObject targetNeck_ind;
    GameObject targetHead_ind;
    GameObject targetleftUpperArm_ind;
    GameObject targetleftLowerArm_ind;
    GameObject targetleftHand_ind;
    GameObject targetrightUpperArm_ind;
    GameObject targetrightLowerArm_ind;
    GameObject targetrightHand_ind;
    GameObject targetrightMidFinger_ind;
    GameObject targetleftUpperLeg_ind;
    GameObject targetleftLowerLeg_ind;
    GameObject targetleftFoot_ind;
    GameObject targetleftToes_ind;
    GameObject targetrightUpperLeg_ind;
    GameObject targetrightLowerLeg_ind;
    GameObject targetrightFoot_ind;
    GameObject targetrightToes_ind;

    GameObject targetleftThumbDistal_ind;
    GameObject targetleftThumbIntermediate_ind;
    GameObject targetleftThumbProximal_ind;
    GameObject targetleftIndexDistal_ind;
    GameObject targetleftIndexIntermediate_ind;
    GameObject targetleftIndexProximal_ind;
    GameObject targetleftMiddleDistal_ind;
    GameObject targetleftMiddleIntermediate_ind;
    GameObject targetleftMiddleProximal_ind;
    GameObject targetleftRingDistal_ind;
    GameObject targetleftRingIntermediate_ind;
    GameObject targetleftRingProximal_ind;
    GameObject targetleftLittleDistal_ind;
    GameObject targetleftLittleIntermediate_ind;
    GameObject targetleftLittleProximal_ind;
    GameObject targetrightThumbDistal_ind;
    GameObject targetrightThumbIntermediate_ind;
    GameObject targetrightThumbProximal_ind;
    GameObject targetrightIndexDistal_ind;
    GameObject targetrightIndexIntermediate_ind;
    GameObject targetrightIndexProximal_ind;
    GameObject targetrightMiddleDistal_ind;
    GameObject targetrightMiddleIntermediate_ind;
    GameObject targetrightMiddleProximal_ind;
    GameObject targetrightRingDistal_ind;
    GameObject targetrightRingIntermediate_ind;
    GameObject targetrightRingProximal_ind;
    GameObject targetrightLittleDistal_ind;
    GameObject targetrightLittleIntermediate_ind;
    GameObject targetrightLittleProximal_ind;


    public GameObject targetHips;
    public GameObject targetSpine;
    public GameObject targetNeck;
    public GameObject targetHead;
    public GameObject targetleftUpperArm;
    public GameObject targetleftLowerArm;
    public GameObject targetleftHand;
    public GameObject targetrightUpperArm;
    public GameObject targetrightLowerArm;
    public GameObject targetrightHand;
    public GameObject targetleftUpperLeg;
    public GameObject targetleftLowerLeg;
    public GameObject targetleftFoot;
    public GameObject targetleftToes;
    public GameObject targetrightUpperLeg;
    public GameObject targetrightLowerLeg;
    public GameObject targetrightFoot;
    public GameObject targetrightToes;

    public GameObject targetleftThumbDistal;
    public GameObject targetleftThumbIntermediate;
    public GameObject targetleftThumbProximal;
    public GameObject targetleftIndexDistal;
    public GameObject targetleftIndexIntermediate;
    public GameObject targetleftIndexProximal;
    public GameObject targetleftMiddleDistal;
    public GameObject targetleftMiddleIntermediate;
    public GameObject targetleftMiddleProximal;
    public GameObject targetleftRingDistal;
    public GameObject targetleftRingIntermediate;
    public GameObject targetleftRingProximal;
    public GameObject targetleftLittleDistal;
    public GameObject targetleftLittleIntermediate;
    public GameObject targetleftLittleProximal;
    public GameObject targetrightThumbDistal;
    public GameObject targetrightThumbIntermediate;
    public GameObject targetrightThumbProximal;
    public GameObject targetrightIndexDistal;
    public GameObject targetrightIndexIntermediate;
    public GameObject targetrightIndexProximal;
    public GameObject targetrightMiddleDistal;
    public GameObject targetrightMiddleIntermediate;
    public GameObject targetrightMiddleProximal;
    public GameObject targetrightRingDistal;
    public GameObject targetrightRingIntermediate;
    public GameObject targetrightRingProximal;
    public GameObject targetrightLittleDistal;
    public GameObject targetrightLittleIntermediate;
    public GameObject targetrightLittleProximal;


    float leglength;
    float Upperleglength;
    float Lowerleglength;

    bool hasLeftToes = false;
    bool hasRightToes = false;
    bool hasLeftfingers = false;
    bool hasRightfingers = false;


    public bool ShowIndicator = false;


    Animator _animationTarget;
    Avatar avatar;

    [Range(0.3f, 1.0f)]
    public float LegExtensionAdjust = 1.0f;
    [Range(0.3f, 1.0f)]
    public float ArmExtensionAdjust = 1.0f;

    private void Awake()
    {
        if (humanoid != null)
        {
            _animationTarget = humanoid.GetComponent<Animator>();
            avatar = _animationTarget.avatar;


            //Update once to get 
            SampleAnimation();
            UpdateTargets();
            CreateTargetObjectsIndicator();

            Upperleglength = (Vector3.Distance(leftUpperLeg.position, leftLowerLeg.position) + Vector3.Distance(rightUpperLeg.position, rightLowerLeg.position)) / 2;
            Lowerleglength = (Vector3.Distance(leftLowerLeg.position, leftFoot.position) + Vector3.Distance(rightLowerLeg.position, rightFoot.position)) / 2;

            //Calculate Leg length
            leglength = (Vector3.Distance(leftUpperLeg.position, leftLowerLeg.position) + Vector3.Distance(leftLowerLeg.position, leftFoot.position)
                + (Vector3.Distance(rightUpperLeg.position, rightLowerLeg.position) + Vector3.Distance(rightLowerLeg.position, rightFoot.position))) / 2;
            Debug.Log("LegLength : " + leglength + "meter");
        }
        else
        {
            Debug.LogWarning("No gameObject assign");
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SampleAnimation();
        CalculateLimbsVectors();
        UpdateTargets();
        UpdateTargetsIndicator();
    }



    void SampleAnimation()
    {
        if (_animationTarget == null)
        {
            Debug.Log("No animator exist.");
            return;
        }
        if (avatar == null || !avatar.isHuman || !avatar.isValid)
        {
            Debug.Log("Avatar is not Humanoid or it is not valid");
            return;
        }
        AnimationRoot = humanoid.transform;
        Hips = _animationTarget.GetBoneTransform(HumanBodyBones.Hips);
        Spine = _animationTarget.GetBoneTransform(HumanBodyBones.Spine);
        Neck = _animationTarget.GetBoneTransform(HumanBodyBones.Neck);
        Head = _animationTarget.GetBoneTransform(HumanBodyBones.Head);

        leftUpperArm = _animationTarget.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        leftLowerArm = _animationTarget.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        leftHand = _animationTarget.GetBoneTransform(HumanBodyBones.LeftHand);

        rightUpperArm = _animationTarget.GetBoneTransform(HumanBodyBones.RightUpperArm);
        rightLowerArm = _animationTarget.GetBoneTransform(HumanBodyBones.RightLowerArm);
        rightHand = _animationTarget.GetBoneTransform(HumanBodyBones.RightHand);

        leftUpperLeg = _animationTarget.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        leftLowerLeg = _animationTarget.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        leftFoot = _animationTarget.GetBoneTransform(HumanBodyBones.LeftFoot);
        leftToes = _animationTarget.GetBoneTransform(HumanBodyBones.LeftToes);
        if (leftToes != null) { hasLeftToes = true; }

        rightUpperLeg = _animationTarget.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        rightLowerLeg = _animationTarget.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        rightFoot = _animationTarget.GetBoneTransform(HumanBodyBones.RightFoot);
        rightToes = _animationTarget.GetBoneTransform(HumanBodyBones.RightToes);
        if (rightToes != null) { hasRightToes = true; }


        leftThumbDistal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftThumbDistal);
        leftThumbIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);
        leftThumbProximal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftThumbProximal);
        leftIndexDistal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftIndexDistal);
        leftIndexIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate);
        leftIndexProximal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftIndexProximal);
        leftMiddleDistal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftMiddleDistal);
        leftMiddleIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate);
        leftMiddleProximal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);
        leftRingDistal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftRingDistal);
        leftRingIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.LeftRingIntermediate);
        leftRingProximal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftRingProximal);
        leftLittleDistal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftLittleDistal);
        leftLittleIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate);
        leftLittleProximal = _animationTarget.GetBoneTransform(HumanBodyBones.LeftLittleProximal);
        if ((leftThumbDistal ?? leftThumbIntermediate ?? leftThumbProximal
            ?? leftIndexDistal ?? leftIndexIntermediate ?? leftIndexProximal
            ?? leftMiddleDistal ?? leftMiddleIntermediate ?? leftMiddleProximal
            ?? leftRingDistal ?? leftRingIntermediate ?? leftRingProximal
            ?? leftLittleDistal ?? leftLittleIntermediate ?? leftLittleProximal) != null) hasLeftfingers = true;


        rightThumbDistal = _animationTarget.GetBoneTransform(HumanBodyBones.RightThumbDistal);
        rightThumbIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);
        rightThumbProximal = _animationTarget.GetBoneTransform(HumanBodyBones.RightThumbProximal);
        rightIndexDistal = _animationTarget.GetBoneTransform(HumanBodyBones.RightIndexDistal);
        rightIndexIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.RightIndexIntermediate);
        rightIndexProximal = _animationTarget.GetBoneTransform(HumanBodyBones.RightIndexProximal);
        rightMiddleDistal = _animationTarget.GetBoneTransform(HumanBodyBones.RightMiddleDistal);
        rightMiddleIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate);
        rightMiddleProximal = _animationTarget.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
        rightRingDistal = _animationTarget.GetBoneTransform(HumanBodyBones.RightRingDistal);
        rightRingIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.RightRingIntermediate);
        rightRingProximal = _animationTarget.GetBoneTransform(HumanBodyBones.RightRingProximal);
        rightLittleDistal = _animationTarget.GetBoneTransform(HumanBodyBones.RightLittleDistal);
        rightLittleIntermediate = _animationTarget.GetBoneTransform(HumanBodyBones.RightLittleIntermediate);
        rightLittleProximal = _animationTarget.GetBoneTransform(HumanBodyBones.RightLittleProximal);
        if ((rightThumbDistal ?? rightThumbIntermediate ?? rightThumbProximal
            ?? rightIndexDistal ?? rightIndexIntermediate ?? rightIndexProximal
            ?? rightMiddleDistal ?? rightMiddleIntermediate ?? rightMiddleProximal
            ?? rightRingDistal ?? rightRingIntermediate ?? rightRingProximal
            ?? rightLittleDistal ?? rightLittleIntermediate ?? rightLittleProximal) != null) hasRightfingers = true;


    }

    void UpdateTargets()
    {
        CopyTransform(targetHips, Hips);

        //Leg Adjust
        if (LegExtensionAdjust == 1.0f)
        {
            CopyTransform(targetleftUpperLeg, leftUpperLeg);
            CopyTransform(targetleftLowerLeg, leftLowerLeg);
            CopyTransform(targetleftFoot, leftFoot);
            if (hasLeftToes) CopyTransform(targetleftToes, leftToes);

            CopyTransform(targetrightUpperLeg, rightUpperLeg);
            CopyTransform(targetrightLowerLeg, rightLowerLeg);
            CopyTransform(targetrightFoot, rightFoot);
            if (hasRightToes) CopyTransform(targetrightToes, rightToes);
        }
        else
        {
            AdjustLeg(LegExtensionAdjust);
        }

        //Hand Adjust
        if(ArmExtensionAdjust == 1.0f)
        {
            CopyTransform(targetleftUpperArm, leftUpperArm);
            CopyTransform(targetleftHand, leftHand);

            CopyTransform(targetrightUpperArm, rightUpperArm);
            CopyTransform(targetrightHand, rightHand);
        }
        else
        {
            AdjustArm(ArmExtensionAdjust);
        }
    }

    void CreateTargetObjectsIndicator()
    {
        targetHips_ind = Instantiate(target_indicator, targetHips.transform.position, Quaternion.identity);
        targetHips_ind.name = "targetHips";

        //targetSpine_ind;
        //targetNeck_ind;
        //targetHead_ind;

        targetleftUpperArm_ind = Instantiate(target_indicator, targetleftUpperArm.transform.position, Quaternion.identity);
        targetleftUpperArm_ind.name = "targetleftUpperArm";
        targetleftHand_ind = Instantiate(target_indicator, targetleftHand.transform.position, Quaternion.identity);
        targetleftHand_ind.name = "targetleftHand";

        targetrightUpperArm_ind = Instantiate(target_indicator, targetrightUpperArm.transform.position, Quaternion.identity);
        targetrightUpperArm_ind.name = "targetrightUpperArm";
        targetrightHand_ind = Instantiate(target_indicator, targetrightHand.transform.position, Quaternion.identity);
        targetrightHand_ind.name = "targetrightHand";

        targetleftUpperLeg_ind = Instantiate(target_indicator, targetleftUpperLeg.transform.position, Quaternion.identity);
        targetleftUpperLeg_ind.name = "targetleftUpperLeg";
        targetleftFoot_ind = Instantiate(target_indicator, targetleftFoot.transform.position, Quaternion.identity);
        targetleftFoot_ind.name = "targetleftFoot";
        targetleftToes_ind = Instantiate(target_indicator, targetleftToes.transform.position, Quaternion.identity);
        targetleftToes_ind.name = "targetleftToes";

        targetrightUpperLeg_ind = Instantiate(target_indicator, targetrightUpperLeg.transform.position, Quaternion.identity);
        targetrightUpperLeg_ind.name = "targetrightUpperLeg";
        targetrightFoot_ind = Instantiate(target_indicator, targetrightFoot.transform.position, Quaternion.identity);
        targetrightFoot_ind.name = "targetrightFoot";
        targetrightToes_ind = Instantiate(target_indicator, targetrightToes.transform.position, Quaternion.identity);
        targetrightToes_ind.name = "targetrightToes";
    }

    void UpdateTargetsIndicator()
    {
        targetHips_ind.transform.position = targetHips.transform.position;
        targetHips_ind.transform.rotation = targetHips.transform.rotation;

        //Left Arm
        targetleftUpperArm_ind.transform.position = targetleftUpperArm.transform.position;
        targetleftHand_ind.transform.position = targetleftHand.transform.position;
        targetleftHand_ind.transform.rotation = targetleftHand.transform.rotation;

        //Right Arm
        targetrightUpperArm_ind.transform.position = targetrightUpperArm.transform.position;
        targetrightHand_ind.transform.position = targetrightHand.transform.position;
        targetrightHand_ind.transform.rotation = targetrightHand.transform.rotation;

        //Left Leg
        targetleftUpperLeg_ind.transform.position = targetleftUpperLeg.transform.position;
        targetleftFoot_ind.transform.position = targetleftFoot.transform.position;
        if (!hasLeftToes) targetleftFoot_ind.transform.rotation = targetleftFoot.transform.rotation;
        if (hasLeftToes) targetleftToes_ind.transform.position = targetleftToes.transform.position;
        if (hasLeftToes) targetleftToes_ind.transform.rotation = targetleftToes.transform.rotation;

        //Right Leg
        targetrightUpperLeg_ind.transform.position = targetrightUpperLeg.transform.position;
        targetrightFoot_ind.transform.position = targetrightFoot.transform.position;
        if (!hasRightToes) targetrightFoot_ind.transform.rotation = targetrightFoot.transform.rotation;
        if (hasRightToes) targetrightToes_ind.transform.position = targetrightToes.transform.position;
        if (hasRightToes) targetrightToes_ind.transform.rotation = targetrightToes.transform.rotation;
    }

    void CalculateLimbsVectors()
    {
        Vector3 leftLegPoleVector = leftLowerLeg.position - CalculateMidProjection(leftUpperLeg, leftLowerLeg, leftFoot);
        Vector3 rightLegPoleVector = rightLowerLeg.position - CalculateMidProjection(rightUpperLeg, rightLowerLeg, rightFoot);
        Vector3 leftArmPoleVector = leftLowerArm.position - CalculateMidProjection(leftUpperArm, leftLowerArm, leftHand);
        Vector3 rightArmPoleVector = rightLowerArm.position - CalculateMidProjection(rightUpperArm, rightLowerArm, rightHand);
    }

    void AdjustArm(float ArmExtensionAdjust)
    {
        Vector3 leftarmvector = leftHand.position - leftUpperArm.position;
        targetleftHand.transform.position = leftUpperArm.position + (leftarmvector * ArmExtensionAdjust);

        Vector3 rightarmvector = rightHand.position - rightUpperArm.position;
        targetrightHand.transform.position = rightUpperArm.position + (rightarmvector * ArmExtensionAdjust);

        
    }

    void AdjustFinger()
    {

    }
    void AdjustBody(float BodyExtensionAdjust)
    {
        //targetHips = Hips;
        Vector3 BodyVector = Neck.position - Hips.position;
    }
    void AdjustLeg(float LegExtensionAdjust)
    {
        Vector3 leftlegvector = leftFoot.position - leftUpperLeg.position;
        targetleftFoot.transform.position = leftUpperLeg.position + (leftlegvector * LegExtensionAdjust);
        if (hasLeftToes) targetleftToes.transform.position = targetleftFoot.transform.position + (leftToes.position - leftFoot.position);

        Vector3 rightlegvector = rightFoot.position - rightUpperLeg.position;
        targetrightFoot.transform.position = rightUpperLeg.position + (rightlegvector * LegExtensionAdjust);
        if (hasRightToes) targetrightToes.transform.position = targetrightFoot.transform.position + (rightToes.position - rightFoot.position);
    }
    
    Vector3 CalculateMidProjection(Transform Root, Transform Mid, Transform End)
    {
        //Calculate the project point A + dot(AP,AB) / dot(AB,AB) * AB
        //float Length = Vector3.Distance(End.position, Root.position);
        Vector3 LimbVector = End.position - Root.position;
        Vector3 MidVector = Mid.position - Root.position;
        Vector3 MidProjPt = Root.position + Vector3.Dot(MidVector, LimbVector) / Vector3.Dot(LimbVector, LimbVector) * LimbVector;
        return MidProjPt;
    }

    Vector3 CalculateMidPointPosition(Transform Root, Transform Mid, Transform End, float Extension, Vector3 PoleVector)
    {
        float limblength = Vector3.Distance(Mid.position, Root.position);
        Vector3 LimbVector = End.position - Root.position;
        float sine = Mathf.Sqrt(1 - Mathf.Pow(((LimbVector * Extension).magnitude / limblength), 2));
        Vector3 MidPointPosition = Root.position + Vector3.Normalize(PoleVector) * limblength * sine; 
        return MidPointPosition;
    }


    void OnDrawGizmos()
    {
        //
        /*
        if (targetHips != null)
        {
            Gizmos.DrawWireSphere(targetHips.position, 0.01f);
            Gizmos.DrawWireSphere(targetWaist.position, 0.01f);
            Gizmos.DrawWireSphere(targetSpine.position, 0.01f);
            Gizmos.DrawWireSphere(targetNeck.position, 0.01f);
            Gizmos.DrawWireSphere(targetHead.position, 0.01f);


            Gizmos.DrawWireSphere(targetleftUpperLeg.position, 0.01f);
            Gizmos.DrawWireSphere(targetleftFoot.position, 0.01f);
            Gizmos.DrawWireSphere(targetrightUpperLeg.position, 0.01f);
            Gizmos.DrawWireSphere(targetrightFoot.position, 0.01f);
            Gizmos.DrawWireSphere(targetleftUpperArm.position, 0.01f);
            Gizmos.DrawWireSphere(targetleftHand.position, 0.01f);
            Gizmos.DrawWireSphere(targetrightUpperArm.position, 0.01f);
            Gizmos.DrawWireSphere(targetrightHand.position, 0.01f);
        }*/
        
    }

    void CopyTransform(GameObject obj, Transform transform)
    {
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
    }
}
