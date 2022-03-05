using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sculpturing.Tools
{
    public class HammerEffects : RockDebrisEffect
    {
        
        public void Emit(Vector3 position)
        {
            Spawn(position);
        }

    }
}