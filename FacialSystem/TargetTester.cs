using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTester : MonoBehaviour
{
    public LookTarget lookTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lookTarget.GetPosition();
    }
}
