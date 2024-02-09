using Live2D.Cubism.Framework.LookAt;
using UnityEngine;

public class LookTarget : MonoBehaviour, ICubismLookTarget
{

    public Camera cam;
    public Vector3 targetPosition;
    public Vector3 default_pos;
    public Vector3 mouse_pos;
    [SerializeField] private VRControllerHandler vrControl;
    [SerializeField] private float trackpadSens = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private bool xMirror = false;

    private bool isAnimationOverride = false;
    private Vector2 overrideDirection;
    private float overrideMoveRate = 5f;

    private void Start()
    {
        
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    public void AnimationOverride(Vector2 direction)
    {
        isAnimationOverride = true;
        overrideDirection = direction;
    }

    public void StopAnimationOverride()
    {
        isAnimationOverride = false;
    }

    public Vector3 GetPosition()
    {
        default_pos = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FixedEyeBall"))
        {
            targetPosition = default_pos;
            return default_pos;
        }

        if (isAnimationOverride)
        {
            Debug.Log("[LookTarget] Animation Overriding");
            targetPosition = default_pos + new Vector3(overrideDirection.x, overrideDirection.y, 0) * overrideMoveRate;
            return targetPosition;
        }

        if (Input.GetMouseButton(0))
        {

            mouse_pos = Input.mousePosition;
            var origin = cam.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.z));
            var direction = cam.transform.forward;
            var hit = new RaycastHit();
            if (Physics.Raycast(origin, Vector3.forward, out hit))
            {
                targetPosition = hit.point;
            }
            Debug.DrawRay(origin, direction * 10f);
            return targetPosition;
        }
        else if (vrControl.GetTouchRightTrackPad())
        {
            var cartesian = vrControl.GetCartesianRightTrackPad();
            var radius = Vector3.Distance(cam.ScreenToWorldPoint(new Vector3(0, 0, 0)), cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0))) / 2 * trackpadSens;
            cartesian = new Vector2(Mathf.Lerp(-radius, radius, cartesian.x / 2 + 0.5f), Mathf.Lerp(-radius, radius, cartesian.y / 2 + 0.5f));
            if (xMirror)
            {
                return new Vector3(-cartesian.x, cartesian.y, default_pos.z);
            }
            else
            {
                return new Vector3(cartesian.x, cartesian.y, default_pos.z);
            }
        }
        else
        {
            targetPosition = default_pos; 
            return default_pos; 
        }
    }


    public bool IsActive()
    {
        return true;
    }
}