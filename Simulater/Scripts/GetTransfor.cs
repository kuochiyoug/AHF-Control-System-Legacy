using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTransfor : MonoBehaviour
{
    public GameObject one;
    public GameObject two;

    public Vector3 TwoRelateOne;
    public Quaternion TwoRelateOneRot;
    public Vector3 OneRelateTwo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TwoRelateOne = one.transform.InverseTransformPoint(two.transform.position);
        TwoRelateOneRot = Quaternion.Inverse(one.transform.rotation) * two.transform.rotation;            
        OneRelateTwo = two.transform.InverseTransformPoint(one.transform.position);
    }
}
