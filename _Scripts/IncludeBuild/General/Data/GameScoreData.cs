using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace General.Data
{
    [CreateAssetMenu(fileName = "ScoreData", menuName = "ScriptableObjects/ScoreData", order = 1)]
    public class GameScoreData : ScriptableObject
    {
        [Space(5)]
        public ScoreData scoresData;
        [Space(5)]
        public ThresholdData thresholdData;
    }
}