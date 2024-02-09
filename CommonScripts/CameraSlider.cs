using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlider : MonoBehaviour
{
    public List<Transform> targets;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        XPosAve();
    }
    [ContextMenu("Excute X Postition Slide")]
    private void XPosAve()
    {
        float xSum = 0.0f;
        foreach (var t in targets)
        {
            xSum += t.position.x;
        }
        transform.position = new Vector3(xSum / targets.Count, transform.position.y, transform.position.z);
    }
}
