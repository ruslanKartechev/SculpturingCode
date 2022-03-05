using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;

namespace Sculpturing.Tools
{
    
    public class ScrabbingEffects : RockDebrisEffect
    {
       

        [Header("Dust component")]
        [SerializeField] private RayfireDust rfDust;
        [Space(10)]
        [SerializeField] private bool EmitDust;

        public void Emit(Vector3 sourcePosition)
        {
            Spawn(sourcePosition);
            if (EmitDust == true) {
                rfDust.gameObject.transform.position = sourcePosition;
                rfDust.Emit();
            }
        }






    }
}