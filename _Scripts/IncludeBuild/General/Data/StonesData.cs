using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Data
{
    [CreateAssetMenu(fileName = "StonesData", menuName = "ScriptableObjects/StonesData", order = 1)]
    public class StonesData : ScriptableObject
    {
        public List<StoneBreakingData> stoneBreakingData = new List<StoneBreakingData>();

    }
}
