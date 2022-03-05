using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace General
{
    public class CameraShake : ShakingHandler
    {
        private CameraManager manager;
        public void Init(CameraManager _manager, Transform _target)
        {
            manager = _manager;
            target = _target;
        }

     

    }
}
