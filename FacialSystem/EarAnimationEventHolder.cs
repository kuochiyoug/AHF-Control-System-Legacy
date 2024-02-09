using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarAnimationEventHolder : MonoBehaviour
{
    [SerializeField] private SceneReference sceneReference;
    [Header("Auto Ear Move")]

    [SerializeField, Range(1f, 10f)]
    public float Mean = 2.5f;

    [SerializeField, Range(0.5f, 5f)]
    public float MaximumDeviation = 2f;

    [SerializeField, Range(1f, 20f)]
    public float Timescale = 10f;

    [SerializeField]
    public float period = 0.25f;

    private Animator animator;
    private MechEarControl earControl;
    private float T;

    // Start is called before the first frame update
    void Start()
    {
        GetMechEar();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        // Fail silently.
        if (earControl == null)
        {
            return;
        }


        // Wait for time until blink.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            T -= Time.deltaTime;


            if (T < 0f)
            {
                T = Mean + Random.Range(-MaximumDeviation, MaximumDeviation);
                StartCoroutine(MechEarPICOPICO());
            }
            else
            {
                return;
            }
        }
    }

    private IEnumerator MechEarPICOPICO()
    {
        earControl.EarFast(MechEarControl.EarState.Middle, MechEarControl.EarState.Middle);
        yield return new WaitForSeconds(period);

        earControl.EarFast(MechEarControl.EarState.Up, MechEarControl.EarState.Up);
        yield return new WaitForSeconds(period);

        earControl.EarFast(MechEarControl.EarState.Middle, MechEarControl.EarState.Middle);
        yield return new WaitForSeconds(period);

        earControl.EarFast(MechEarControl.EarState.Up, MechEarControl.EarState.Up);
        yield return new WaitForSeconds(period);
    }

    public void BothEarFastControl(MechEarControl.EarState state)
    {
        if (!earControl)
        {
            return;
        }
        earControl.EarFast(state, state);
    }
    public void BothEarSlowControl(MechEarControl.EarState state)
    {
        if (!earControl)
        {
            return;
        }
        earControl.EarSlow(state, state);
    }
    public void RightEarSlowControl(MechEarControl.EarState state)
    {
        if (!earControl)
        {
            return;
        }
        earControl.EarSlow(state, earControl.GetEarStates()[1]);
    }
    public void LeftEarSlowControl(MechEarControl.EarState state)
    {
        if (!earControl)
        {
            return;
        }
        earControl.EarSlow(earControl.GetEarStates()[0], state);
    }

    private bool GetMechEar()
    {
        earControl =
            CutieroidUtility.SceneUtility
            .GetGameObjectInScene(
                sceneReference.GetScene("MotorControlPanel"),
                "MotorControl"
            )
            .GetComponentInChildren<MechEarControl>();
        return earControl != null;
    }
}
