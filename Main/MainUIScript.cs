using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainUIScript : MonoBehaviour
{
    public MotionAndFacialAPI MotionAndFacial;
    public MotionPlayer mp;
    public AutoMotionPlayer automp;
    public Button Autoplay_button;
    public Slider slider;
    public Text frameCountText;
    public Dropdown motion_dropdown;
    private bool ready = false;
    private bool value_changing = false;
    private int loaded_motion = 0;

    private Image autoplayCheck;

    // Start is called before the first frame update
    void Awake()
    {
        MotionAndFacial = GetComponent<MotionAndFacialAPI>();
        slider.minValue = 0;
        autoplayCheck = Autoplay_button.GetComponentsInChildren<Image>()[1];
    }

    void SlideBarUpdate()
    {
        if (mp != null && mp.has_motion)
        {
            slider.interactable = true;
            slider.maxValue = mp.frames - 1;

            if (!value_changing)
                slider.value = mp.current_frame;
            frameCountText.text = (mp.current_frame.ToString() + "/" + mp.frames.ToString());
        }
        else
        {
            mp = MotionAndFacial.motionPlayer;
        }
    }

    public void DropdownUpdate()
    {
        if (automp.MotionFiles.Count != 0 && automp.MotionFiles.Count != loaded_motion)
        {
            motion_dropdown.ClearOptions();
            motion_dropdown.AddOptions(automp.MotionFiles.Select(x => System.IO.Path.GetFileNameWithoutExtension(x)).ToList());
            loaded_motion = automp.MotionFiles.Count;
        }
    }
    
    void AutoPlayButtonUpdata()
    {
        if (automp.play && !autoplayCheck.enabled)
        {
            autoplayCheck.enabled = true;
        }
        else if (!automp.play && autoplayCheck.enabled)
        {
            autoplayCheck.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SlideBarUpdate();
        DropdownUpdate();
        AutoPlayButtonUpdata();
    }
    public void OnSliderValueChanged()
    {
        if (mp != null && mp.has_motion && value_changing)
        {
            mp.current_frame = (uint)slider.value;
        }
        //mp.SetFramePoint((uint)slider.value);
    }

    public void OnEnterDrag()
    {
        if (mp != null && mp.has_motion)
            mp.Pause();
        value_changing = true;
    }

    public void OnLeaveDrag()
    {
        if (mp != null && mp.has_motion)
            mp.Pause();
        value_changing = false;
    }

}
