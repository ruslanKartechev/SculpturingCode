using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sculpturing.Levels;
namespace General.Data
{


    public class DataManager : MonoBehaviour 
    {
        [SerializeField] private DataGameMain main;
        [SerializeField] private GameScoreData scores;
        [SerializeField] private StonesData stones;
        [SerializeField] private FinishingSequenceData finishingSequence;

        public DataGameMain MainGameData
        { get { return main; } }
        public GameScoreData ScoreData { get { return scores; } }
        public StonesData StonesData { get { return stones; } }

        public FinishingSequenceData FinishingSequence { get { return finishingSequence; } }

        [HideInInspector] public TempStageData tempData;
        public const string CameraPosName = "CameraPos_";

        [HideInInspector] public int rocksSpawnerSpacing;
        [HideInInspector] public LevelInstance currentLevel;
        [HideInInspector] public LevelObjects currentLevelObjects;

    }
}