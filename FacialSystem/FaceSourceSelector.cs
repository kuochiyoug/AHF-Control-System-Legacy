using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceSourceSelector : MonoBehaviour
{
    [SerializeField] private GameObject live2DProjector;
    [SerializeField] private GameObject vrmProjector;
    [SerializeField] private Button toggleButton;

    private bool useLive2D = true;
    private Text buttonText;

    private const string toLive2DText = "Use Live2D Face";
    private const string toVrmText = "Use VRM Face";

    void Start()
    {
        buttonText = toggleButton.GetComponentInChildren<Text>();
        Switch();
    }

    public void Switch()
    {
        useLive2D = !useLive2D;
        buttonText.text = useLive2D ? toVrmText : toLive2DText;
        live2DProjector.SetActive(useLive2D);
        vrmProjector.SetActive(!useLive2D);
    }
}
