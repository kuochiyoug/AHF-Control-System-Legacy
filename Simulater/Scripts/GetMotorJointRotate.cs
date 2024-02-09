using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMotorJointRotate : MonoBehaviour
{

    public ObserveAxis rotate_axis;
    public float zero_point;

    public float angle;

    [HideInInspector]
    public string JointName;

    public GameObject targetObject;

    // public Quaternion rotationDelta;
    public Quaternion InitialRotation;

    private void Awake()
    {
        if (targetObject != null)
            InitialRotation = Quaternion.FromToRotation(this.transform.forward, targetObject.transform.forward);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (rotate_axis == ObserveAxis.x)
        {

        }
        else if (rotate_axis == ObserveAxis.y)
        {

        }
        else if (rotate_axis == ObserveAxis.z)
        {

        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            Quaternion rotationDelta = Quaternion.FromToRotation(this.transform.forward, targetObject.transform.forward);
            Vector3 rotationDeltaEuler = InitialRotation.eulerAngles - rotationDelta.eulerAngles;
            angle = rotationDeltaEuler.z;
        }
    }

    float GetXAngle()
    {
        angle = this.transform.rotation.eulerAngles.x;
        float angle_chid = this.gameObject.transform.GetChild(0).rotation.eulerAngles.x;
        Debug.Log(angle_chid);
        return angle;
    }
    float GetYAngle()
    {
        angle = this.transform.rotation.eulerAngles.y;
        float angle_chid = this.gameObject.transform.GetChild(0).rotation.eulerAngles.x;
        Debug.Log(angle_chid);
        return angle;
    }
    float GetZAngle()
    {
        angle = this.transform.rotation.eulerAngles.z;
        float angle_chid = this.gameObject.transform.GetChild(0).rotation.eulerAngles.x;
        Debug.Log(angle_chid);
        return angle;
    }

    public enum ObserveAxis
    {
        x,
        y,
        z
    };

}
