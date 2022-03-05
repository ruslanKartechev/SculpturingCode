using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using Sculpturing.Tools;
using General;
namespace Sculpturing.Levels
{
    public class StageManager : MonoBehaviour
    {
        public StageLoader stageLoader;
        public StageProgress stageProgress;
        public List<StageData> stages { get; private set; }
        public int CurrentStage { get; private set; }
        private LevelObjects currentLevel;
        [Space(10f)]
        [SerializeField] private bool TestSingleScene = false;
        [SerializeField] private int TestStageNum;

        public void Init()
        {
            CurrentStage = 0;
            stages = new List<StageData>();
            stages = GameManager.Instance.data.MainGameData.Stages;
            stageProgress.Init(this);
            if (stages ==null || stages.Count == 0) { Debug.LogWarning("Stages are not defined");return; }
            GameManager.Instance.eventManager.CameraStartPositionSet.AddListener(InitStages);
            GameManager.Instance.eventManager.StageFinished.AddListener(FinishStage);
            GameManager.Instance.eventManager.ToolInited.AddListener(OnToolInited);
        }
        private void OnToolInited(Tool tool)
        {
            GameManager.Instance.controlls.ResumeInput();
        }
        private void InitStages()
        {
            currentLevel = GameManager.Instance.data.currentLevelObjects;
            if (TestSingleScene == false)
                InitLevelFromStart(currentLevel);
            else
                InitSingleStage(currentLevel, TestStageNum);
        }

        private void InitLevelFromStart(LevelObjects levelData)
        {
            if(stages == null)
            {
                stages = new List<StageData>();
                stages = GameManager.Instance.data.MainGameData.Stages;
            }
            CurrentStage = 0;
            currentLevel = levelData;
            stageLoader.Init(this);
            stageLoader.LoadFromStart(levelData);
        }
        private void InitSingleStage(LevelObjects levelData, int stageNum)
        {
            StageData stage = stages.Find(x => x.stageNumber == stageNum);
            if (stage != null)
            {
                CurrentStage = stageNum - 1;
                stageLoader.LoadSingle(levelData, stage);
            }
            else
                Debug.Log("stage not found");
        }

        public void InitCurrent() => GameManager.Instance.eventManager.NewStageLoaded.Invoke(stages[CurrentStage]);
        public void FinishStage()
        {
            StopAllCoroutines();
            StartCoroutine(TransitioningStages());
            GameManager.Instance.controlls.StopInput();
        }

        private IEnumerator TransitioningStages()
        {
            yield return new WaitForSeconds(0.5f);
            if (CurrentStage < stages.Count - 1)
            {
                CurrentStage += 1;
                stageLoader.ClearPreviousStage(stages[CurrentStage]);
                yield return null;
                InitCurrent();
            }
            else
            {
                GameManager.Instance.eventManager.LevelFinishInit.Invoke();
            }
        }
        




    }
}