using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmMappingParamInputReciever : MonoBehaviour
{
    [SerializeField] private ShoulderPositionEstimation estimation;
    public Text placeHolder;
    public InputField inputField;
    public Slider slider;
    public Text correctionRateDisplay;

    // Start is called before the first frame update
    void Start()
    {
        correctionRateDisplay.text = "Correction rate " + Mathf.Clamp(estimation.correctionRate, 0.5f, 1.5f).ToString("F2");
        placeHolder.text = (estimation.heightOfBody * 100) + " cm";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHeightEditEnd(string height)
    {
        float inputHeight;
        if (!float.TryParse(height, out inputHeight))
        {
            return;
        }

        inputField.text = inputHeight + " cm";

        inputHeight /= 100;

        estimation.heightOfBody = inputHeight;
    }

    public void OnSliderValueChange(float value)
    {
        estimation.correctionRate = Mathf.Clamp(value, 0.5f, 1.5f);
        correctionRateDisplay.text = "Correction rate " + Mathf.Clamp(value, 0.5f, 1.5f).ToString("F2");
    }
}
