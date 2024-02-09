using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Co1umbine.SHYMotionRecorder
{
    public interface IMotionLoader
    {
        public void LoadMotion(string directoryName, string fileName);
    }
}