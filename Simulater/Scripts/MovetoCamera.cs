using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetoCamera : MonoBehaviour
{
    public GameObject camera;
    public float tx=0;
    public float ty=0;
    public float tz=0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x=gameObject.transform.position.x;
        float y= gameObject.transform.position.y;
        float z= gameObject.transform.position.z;
        Debug.Log("Moviung to " + x + " " + y + " " + z);

        this.transform.position = new Vector3(x+tx,y+ty,z+tz);
    }
}
