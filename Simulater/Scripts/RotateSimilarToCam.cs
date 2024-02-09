using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSimilarToCam : MonoBehaviour
{
    public GameObject cam;
    public float X_offset = 0;
    public float Y_offset = 0;
    public float Z_offset = 0;


    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x+ X_offset, cam.transform.rotation.eulerAngles.y+ Y_offset, cam.transform.rotation.eulerAngles.z);
    }
}
