using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IKUI : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private GetAngleFromSelf getAngle;

    // Start is called before the first frame update
    void Start()
    {
        toggle.isOn = getAngle.IKControlActivation;
        toggle.onValueChanged.AddListener(OnToggleChange);
    }

    private void OnToggleChange(bool isEnable)
    {
        getAngle.IKControlActivation = isEnable;
    }
}
