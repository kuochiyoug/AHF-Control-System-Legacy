using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Co1umbine.InteractionMotion
{
    public class PoseInitializer : MonoBehaviour
    {
        public Animator animator;

        [SerializeField] private KeyCode initializeKey = KeyCode.I;

        private HumanPoseHandler poseHandler;
        private HumanPose humanPose = new HumanPose();

        private void Start()
        {
            poseHandler = new HumanPoseHandler(animator.avatar, animator.transform);

            poseHandler.GetHumanPose(ref humanPose);
            poseHandler.GetHumanPose(ref humanPose);


        }

        private void Update()
        {
            if (Input.GetKeyDown(initializeKey))
            {
                InitializePose();
            }
        }

        private void InitializePose()
        {
            poseHandler.SetHumanPose(ref humanPose);
        }
    }
}