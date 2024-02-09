using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;



public class GrabTarget : MonoBehaviour
{
    private SteamVR_Input_Sources handType;
    private SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    private GameObject collidingObject; 
    private GameObject objectInHand; 

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        //print("Select " + col.name);
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        //print("Enter! "+other.name);
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
    }

    void Awake()
    {
        controllerPose = gameObject.GetComponent<SteamVR_Behaviour_Pose>();
        handType = controllerPose.inputSource;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            
            if (collidingObject)
            {
                //print(handType + " Grab " + collidingObject.name);
                GrabObject();
            }
        }

        if (grabAction.GetLastStateUp(handType))
        {

            if (objectInHand)
            {
                ReleaseObject();
            }
        }


    }
}
