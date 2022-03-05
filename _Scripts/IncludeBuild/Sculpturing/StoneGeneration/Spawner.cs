using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;
namespace Sculpturing
{
    public class Spawner : MonoBehaviour
    {
        protected Transform shatterRoot;
        protected List<GameObject> shatteredPeieces = new List<GameObject>();
        [Header("Shattering settings")]
        [SerializeField] protected Material mainMaterial;
        [SerializeField] protected Material shatteredMaterial;
   
    }
}