using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float timeScale = 1;
    [SerializeField] Slider slider;
    [SerializeField] Text disp;
    float prevTimeScale = 1;

    string subject = "World Time Scale ";

    private void Start()
    {
        if(timeScale > 1 && timeScale < 0)
        {
            Time.timeScale = timeScale;
        }
        slider.value = Time.timeScale;
    }

    private void Update()
    {
        if (prevTimeScale != timeScale)
        {
            TimeControl(timeScale);
        }
    }

    public void TimeControl(float ts)
    {
        if (ts > 1 || ts < 0)
        {
            return;
        }
        timeScale = ts;
        prevTimeScale = ts;
        Time.timeScale = ts;
        disp.text = subject + ts.ToString("F2");
    }
}
