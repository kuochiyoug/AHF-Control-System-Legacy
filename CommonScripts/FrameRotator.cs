using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRotator : MonoBehaviour
{
    public Vector3 targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(targetRotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
