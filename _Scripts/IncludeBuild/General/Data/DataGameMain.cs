using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Data {
    [CreateAssetMenu(fileName = "DataGameMain", menuName = "ScriptableObjects/DataGameMain", order = 1)]
    public class DataGameMain : ScriptableObject
    {
        public List<StageData> Stages;
        [Space(5)]
        public ToolsData toolsData;
        [Space(5)]
        [Header("UI Editor Ad panel or progress panel")]
        public bool EditorUIMode;
        [Header("Level finish button settings")]
        public bool IsDebug = false;
    }
}