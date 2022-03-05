using System.Collections;
using UnityEngine;
using General.Data;
using Sculpturing.UI;
using General;
namespace Sculpturing.Levels
{
    public class StageLoader : MonoBehaviour
    {
        private StageManager manager;
        public LevelObjects spawnedObjects { get; private set; }

        public void Init(StageManager _manager)
        {
            manager = _manager;
        }

        public void LoadFromStart(LevelObjects levelData)
        {
            spawnedObjects = levelData;
            
            if(spawnedObjects.sculpturingObjects.Tier_1_root == null || spawnedObjects.sculpturingObjects.Tier_2_roots == null)
            {
                Debug.Log("Sculpturing stage objects are not found");
                return;
            }
            if(spawnedObjects.polishingObjects.SpawnedShardsRoot == null)
            {
                Debug.Log("Polishing stage objects are not found");
                return;
            }

            int stage_1_count = spawnedObjects.sculpturingObjects.Tier_1_root.childCount/2;
            int stage_2_count = spawnedObjects.polishingObjects.shards.Count;
            TempStageData data = new TempStageData(stage_1_count, stage_2_count);
            GameManager.Instance.data.tempData = data;
            manager.InitCurrent();
        }


        /// only for debugging,
        /// for loading levels use LoadFromStart
        public void LoadSingle(LevelObjects level, StageData stage)
        {
            if(stage.stageName == StageByName.Sculpturing)
            {
            } else if(stage.stageName == StageByName.Polishing)
            {
            } else if (stage.stageName == StageByName.Painting)
            {
               
            }
        }

        public void ClearPreviousStage(StageData currentStage)
        {

            if(currentStage.stageName == StageByName.Polishing)
            {
                ClearStage_1();
            } else if (currentStage.stageName == StageByName.Painting)
            {
                ClearStage_2();
            }
        }


        private void ClearStage_1()
        {
            if(spawnedObjects.sculpturingObjects.mainStone!=null)
                spawnedObjects.sculpturingObjects.mainStone.ClearComponent();
            spawnedObjects.Stone.gameObject.SetActive(false);
            DestroyImmediate(spawnedObjects.sculpturingObjects.Tier_1_root.gameObject);
            for (int i = 0; i < spawnedObjects.sculpturingObjects.Tier_2_roots.Count; i++)
            {
                if (spawnedObjects.sculpturingObjects.Tier_2_roots[i] != null)
                    DestroyImmediate(spawnedObjects.sculpturingObjects.Tier_2_roots[i].gameObject);
            }
        }
        private void ClearStage_2()
        {
            spawnedObjects.Stone.gameObject.SetActive(false);
            DestroyImmediate(spawnedObjects.polishingObjects.SpawnedShardsRoot);

        }

    }
}