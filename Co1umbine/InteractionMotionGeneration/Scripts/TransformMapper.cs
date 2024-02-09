using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Co1umbine.InteractionMotion
{
    public class TransformMapper : MonoBehaviour
    {
        public Animator actualLeader;
        private HumanPoseHandler alPoseHandler;

        public Animator actualFollwer;
        private HumanPoseHandler afPoseHandler;

        public Animator virtualLeader;
        private HumanPoseHandler vlPoseHandler;

        public Animator virtualFollwer;
        private HumanPoseHandler vfPoseHandler;

        private void Start()
        {
            alPoseHandler = new HumanPoseHandler(actualLeader.avatar, actualLeader.transform);
            afPoseHandler = new HumanPoseHandler(actualFollwer.avatar, actualFollwer.transform);
            vlPoseHandler = new HumanPoseHandler(virtualLeader.avatar, virtualLeader.transform);
            vfPoseHandler = new HumanPoseHandler(virtualFollwer.avatar, virtualFollwer.transform);
        }

        void Update()
        {

        }

        private void SetTransform()
        {
            var alHumanPose = new HumanPose();
            alPoseHandler.GetHumanPose(ref alHumanPose);
            alPoseHandler.GetHumanPose(ref alHumanPose);

        }
    }
}
