using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using Sculpturing.UI;
using General;
namespace Sculpturing.Levels
{
    public class StageProgress : MonoBehaviour
    {
        public float CurrentStageProgress { get; private set; }
        public int CurrentStageScore { get; private set; }
        public int TotalStageScore { get; private set; }
        private int PassThreshold;
        private int Threshold_stage_2;
        private bool threshold_main_reached;
        private bool threshold_stage_2_reached;
        private StageManager manager;
        private string currentStageName;
        public void Init(StageManager _manager)
        {
            manager = _manager;
            CurrentStageProgress = 0;
            CurrentStageScore = 0;
            GameManager.Instance.eventManager.ScoreAdded.AddListener(AddProgress);
            GameManager.Instance.eventManager.NewStageLoaded.AddListener(InitStage);
            PassThreshold = GameManager.Instance.data.ScoreData.thresholdData.MainThreshold;
            Threshold_stage_2 = GameManager.Instance.data.ScoreData.thresholdData.Threshold_stage_2;
        }
        public void InitStage(StageData stage)
        {
            CurrentStageScore = 0;
            CurrentStageProgress = 0;
            currentStageName = stage.stageName;
            threshold_main_reached = false;
            threshold_stage_2_reached = false;
            InitTotalStageScore(currentStageName);
        }

        private void AddProgress(int score)
        {
            if(TotalStageScore == 0) { Debug.Log("Total score = 0"); return; }
            CurrentStageScore += score;
            CurrentStageProgress = (float)CurrentStageScore / TotalStageScore;
            GameManager.Instance.eventManager.ProgressAdded.Invoke(CurrentStageProgress);
            if(CurrentStageProgress * 100 >= PassThreshold && threshold_main_reached == false)
            {
                threshold_main_reached = true;
                GameManager.Instance.eventManager.ThresholdReached.Invoke();
            }     
            if (currentStageName == StageByName.Polishing && CurrentStageProgress * 100 >= Threshold_stage_2 && threshold_stage_2_reached == false)
            {
                threshold_stage_2_reached = true;
                GameManager.Instance.eventManager.Stage_2_thresholdReached.Invoke();
            }
            if (CurrentStageProgress >= 1)
            {
                FinishStage();
                CurrentStageProgress = 0;
            }
        }

        private void FinishStage()
        {
            GameManager.Instance.eventManager.StageFinished.Invoke();
        }
        public void InitTotalStageScore(string stageName)
        {
            if(GameManager.Instance.data.tempData == null)
            {
                Debug.Log("temp data not set");
                return;
            }

            TotalStageScore = 0;
            if (stageName == StageByName.Sculpturing)
            {
                TotalStageScore = GameManager.Instance.data.tempData.StageOneSpawned * GameManager.Instance.data.ScoreData.scoresData.StonePieceBrokenScore;
                TotalStageScore += GameManager.Instance.data.ScoreData.scoresData.BigStoneBrokenScore;
            } else if (stageName == StageByName.Polishing)
            {
                TotalStageScore = GameManager.Instance.data.tempData.StageTwoSpawned * GameManager.Instance.data.ScoreData.scoresData.RockPolishedScore;
            }
            else if (stageName == StageByName.Decorating)
            {
                TotalStageScore = GameManager.Instance.data.ScoreData.scoresData.MaxStickersCount;
            }
           // Debug.Log("Total stage score is set to " + TotalStageScore);
        }

    }
}