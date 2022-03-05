using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Data
{
    [CreateAssetMenu(fileName = "ParticlesData", menuName = "ScriptableObjects/ParticlesData", order = 1)]
    public class ParticlesData : ScriptableObject
    {
        [Tooltip("Debris particles Prefabs. All prefabs must be approximately the same size")]
        public GameObject particlesPF ;
        [Tooltip("Max number of debris particles. Set 4 or more")]
        public int maxCount;
        [Header("Particles size")]
        [Tooltip("Use fixed size scaler or varying (min,max)?")]
        public bool variableSize;
        public float constSize = 0.3f;
        public float minSize = 0.2f;
        public float maxSize = 0.4f;
        [Header("Particles LifeSpan")]
        public float lifeMin = 0.3f;
        public float lifeMax = 1f;

    }
}