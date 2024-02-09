using UnityEngine;
using Valve.VR;

public class GrabActionTest : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Action_Boolean grabAction; // 3


    void Update()
    {
        if (GetGrab())
        {
            print("Grab " + handType);
        }

    }

    public bool GetGrab() // 2
    {
        return grabAction.GetState(handType);
    }
}