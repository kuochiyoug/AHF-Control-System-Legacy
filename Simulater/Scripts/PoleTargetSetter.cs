using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleTargetSetter : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform shoulderTransform;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(handTransform.position.y > shoulderTransform.position.y)
        {
            transform.position = new Vector3(startPosition.x, startPosition.y, -startPosition.z);
        }
        else
        {
            transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }
    }
}
