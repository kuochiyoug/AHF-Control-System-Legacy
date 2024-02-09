using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaknessPositionHolder : MonoBehaviour
{
    public Vector3 weaknessPosition;
    public Vector3 weaknessEuler;
    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float error = 0.1f;
    [SerializeField] SceneReference sceneReference;

    private Coroutine goToWeaknessCoroutine;
    private Vector3 speed = Vector3.zero;
    private Quaternion weaknessRotation;

    private DynamixelMotorAPI dynamixel;



    public void GoToWeaknessPosition()
    {
        if (goToWeaknessCoroutine != null)
        {
            return;
        }
        Debug.Log("Start loop");
        weaknessRotation = Quaternion.Euler(weaknessEuler);
        goToWeaknessCoroutine = StartCoroutine(GoToWeaknessLoop());
    }

    private IEnumerator GoToWeaknessLoop()
    {
        while (Vector3.Distance(transform.position, weaknessPosition) > error || Quaternion.Angle(transform.rotation, weaknessRotation) > error)
        {
            transform.position = MoveTowardVec();
            transform.rotation = RotateTowardRot();
            yield return null;
        }
    }

    private Vector3 MoveTowardVec()
    {
        return Vector3.SmoothDamp(transform.position, weaknessPosition, ref speed, 2, moveSpeed);
    }

    private Quaternion RotateTowardRot()
    {
        return Quaternion.RotateTowards(transform.rotation, weaknessRotation, moveSpeed);
    }
    public void StartTracking()
    {
        if (goToWeaknessCoroutine != null)
        {
            Debug.Log("Stop loop");
            StopCoroutine(goToWeaknessCoroutine);
            goToWeaknessCoroutine = null;
        }
    }
    //public async Task OnApplicationQuit(bool isOn)
    //{
    //    if (dynamixel == null)
    //    {
    //        if (!GetDynamixel())
    //        {
    //            Debug.LogError("Can't reference DynamixelMotorAPI");
    //            return;
    //        }
    //    }

    //    await StartCoroutine(GoToWeaknessLoop());

    //    dynamixel.Disable_torque();
    //}

    private bool GetDynamixel()
    {
        dynamixel =
            CutieroidUtility.SceneUtility.GetGameObjectInScene(
                sceneReference.GetScene("MotorControlPanel"),
                "MotorControl"
            )
            .GetComponentInChildren<DynamixelMotorAPI>();
        return dynamixel != null;
    }
}