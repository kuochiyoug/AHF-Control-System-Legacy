using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionPiMenu : MonoBehaviour
{
    [Header("Scripts"), SerializeField] private VRControllerHandler controller;
    [Header("GameObjects"), SerializeField] private GameObject PiMenu;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private RectTransform center;
    [Tooltip("Top~ clockwise"), SerializeField] private List<RectTransform> buttons;
    [Header("Animation"),SerializeField] private Animator animator;
    [Tooltip("0: center,\n1~8: Top~ clockwise"), SerializeField] private string[] animationNames;

    [Header("Behaviour")]
    public bool useVRControl;
    [Tooltip("radius of Pi Menu")] public float radius = 185f;

    [Header("Debug")]
    public Vector2 centeredMouse;
    [SerializeField] private bool isOpenMenu;

    private int currentIndex = 0;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (useVRControl)
        {
            if (controller.GetTouchLeftTrackPad())
            {
                isOpenMenu = true;
            }
            if (!controller.GetTouchLeftTrackPad())
            {
                if (isOpenMenu)
                {
                    if (currentIndex < animationNames.Length)
                    {
                        PlayAnimation(animationNames[currentIndex]);
                    }
                }
                isOpenMenu = false;
            }
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                isOpenMenu = true;
            }
            if (!Input.GetMouseButton(1))
            {
                if (isOpenMenu)
                {
                    if (currentIndex < animationNames.Length)
                    {
                        PlayAnimation(animationNames[currentIndex]);
                    }
                }
                isOpenMenu = false;
            }
        }

        PiMenuBehaviour();
        CursorBehaviour();

    }

    public void SetPiMenuControlbyVR(bool useVR)
    {
        useVRControl = useVR;
    }

    private Vector2 GetCenteredMousePos()
    {
        return new Vector2(Input.mousePosition.x - Screen.width / 2.0f, Input.mousePosition.y - Screen.height / 2.0f);
    }

    private void CursorBehaviour()
    {
        if (isOpenMenu)
        {
            if (useVRControl)
            {
                var vec2 = controller.GetCartesianLeftTrackPad();
                cursor.anchoredPosition = new Vector2(Mathf.Lerp(-radius, radius, (vec2.x + 1) / 2), Mathf.Lerp(-radius, radius, (vec2.y + 1) / 2));
            }
            else
            {
                var vec2 = GetCenteredMousePos();
                cursor.anchoredPosition = new Vector2(Mathf.Lerp(-radius, radius, Mathf.Clamp(vec2.x, -100, 100) / 200 + 0.5f), Mathf.Lerp(-185, 185, Mathf.Clamp(vec2.y, -100, 100) / 200 + 0.5f));
            }
        }
    }

    private void PiMenuBehaviour()
    {
        if (isOpenMenu)
        {
            if (!PiMenu.activeSelf)
            {
                PiMenu.SetActive(true);
            }

            float r, theta;

            if (useVRControl)
            {
                r = controller.GetPolarVectorLeftTrackPad().x;

                //degree -180 ~ 180
                theta = controller.GetPolarVectorLeftTrackPad().y;
            }
            else
            {
                var vec2 = GetCenteredMousePos();
                var polar = controller.ConversionPolar(new Vector2(Mathf.Clamp(vec2.x, -100, 100) / 100, Mathf.Clamp(vec2.y, -100, 100) / 100));
                r = polar.x;
                theta = polar.y;
            }

            centeredMouse = GetCenteredMousePos();

            if (r < 0.4f)
            {
                SpotButton(0);
                return;
            }
            if (Mathf.Abs(theta) < 22.5f)  // north
            {
                SpotButton(1);
                return;
            }
            else if (Mathf.Abs(theta - 45 * 1f) < 22.5f)  // north_east
            {
                SpotButton(2);
                return;
            }
            else if (Mathf.Abs(theta - 45 * 2f) < 22.5f)  // east
            {
                SpotButton(3);
                return;
            }
            else if (Mathf.Abs(theta - 45 * 3f) < 22.5f)  // south_east
            {
                SpotButton(4);
                return;
            }
            else if (Mathf.Abs(theta - 45 * 4f) < 22.5f)  // south
            {
                SpotButton(5);
                return;
            }
            else if (Mathf.Abs(theta + 45 * 1f) < 22.5f)  // north_weast
            {
                SpotButton(8);
                return;
            }
            else if (Mathf.Abs(theta + 45 * 2f) < 22.5f)  // weast
            {
                SpotButton(7);
                return;
            }
            else if (Mathf.Abs(theta + 45 * 3f) < 22.5f)  // south_weast
            {
                SpotButton(6);
                return;
            }
        }
        else
        {
            if (PiMenu.activeSelf)
            {
                PiMenu.SetActive(false);
            }
        }
    }

    private void SpotButton(int index)
    {
        currentIndex = index;
        index--;

        if (index == -1)
        {
            center.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
        else
        {
            center.localScale = new Vector3(1f, 1f, 1f);
        }
        for (int i = 0; i < 8; i++)
        {
            if (index == i)
            {
                buttons[i].localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
            else
            {
                buttons[i].localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    public void PlayAnimation(string animationName)
    {
        if(animator == null)
        {
            animator = this.GetComponent<Animator>();
        }
        animator.Play(animationName, -1);
    }
}
