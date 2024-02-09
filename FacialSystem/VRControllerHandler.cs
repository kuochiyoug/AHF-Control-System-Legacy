using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRControllerHandler : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean touch;
    [SerializeField] private SteamVR_Action_Boolean select;
    [SerializeField] private SteamVR_Action_Vector2 position;
    [SerializeField] private SteamVR_Action_Boolean trigger;

    [SerializeField] private SteamVR_Input_Sources righthandType;
    [SerializeField] private SteamVR_Input_Sources lefthandType;

    public bool isTouch;
    public bool click;
    public bool leftPinch;
    public bool rightPinch;
    public Vector2 cartesian;

    private void Start()
    {
    }
    public bool GetTouchRightTrackPad()
    {
        return touch.GetState(righthandType);
    }
    public bool GetTouchLeftTrackPad()
    {
        return touch.GetState(lefthandType);
    }

    public bool GetSelectRightTrackPad()
    {
        return select.GetStateDown(righthandType);
    }
    public bool GetSelectLeftTrackPad()
    {
        return select.GetStateDown(lefthandType);
    }

    public bool GetTriggerRight()
    {
        return trigger.GetState(righthandType);
    }
    public bool GetTriggerRightDown()
    {
        return trigger.GetStateDown(righthandType);
    }
    public bool GetTriggerLeft()
    {
        return trigger.GetState(lefthandType);
    }
    public bool GetTriggerLeftDown()
    {
        return trigger.GetStateDown(lefthandType);
    }

    //-----Integrate right and left-----
    public Vector2 GetCartesianIntegrateTrackPad()
    {
        var right = position.GetAxis(righthandType);
        if (!GetTouchRightTrackPad())
        {
            right = Vector2.zero;
        }
        var left = position.GetAxis(lefthandType);
        if (!GetTouchLeftTrackPad())
        {
            left = Vector2.zero;
        }

        cartesian = new Vector2(right.x + left.x, right.y + left.y);

        return cartesian.magnitude <= 1 ? cartesian : cartesian.normalized;
    }

    public Vector2 GetPolarVectorIntegrateTrackPad()
    {
        return ConversionPolar(GetCartesianIntegrateTrackPad());
    }

    //-----Right-----
    public Vector2 GetCartesianRightTrackPad()
    {
        return cartesian = position.GetAxis(righthandType);
    }

    public Vector2 GetPolarVectorRightTrackPad()
    {
        return ConversionPolar(GetCartesianRightTrackPad());
    }

    //-----Left-----
    public Vector2 GetCartesianLeftTrackPad()
    {
        return cartesian = position.GetAxis(lefthandType);
    }

    public Vector2 GetPolarVectorLeftTrackPad()
    {
        return ConversionPolar(GetCartesianLeftTrackPad());
    }
    //----------

    public Vector2 ConversionPolar(Vector2 cartesian)
    {
        var theta = Mathf.Asin(cartesian.normalized.x);
        if (cartesian.y < 0)
        {
            if (cartesian.x >= 0)
            {
                theta = Mathf.PI - theta;
            }
            else
            {
                theta = -Mathf.PI - theta;
            }
        }
        return new Vector2(cartesian.magnitude, theta * Mathf.Rad2Deg);
    }

    void Update()
    {
        isTouch = GetTouchLeftTrackPad() || GetTouchRightTrackPad();
        click = GetSelectLeftTrackPad() || GetSelectRightTrackPad();
        leftPinch = GetTriggerLeft();
        rightPinch = GetTriggerRight(); 
    }
}
