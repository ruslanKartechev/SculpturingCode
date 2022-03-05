using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;

namespace Sculpturing.Tools
{
    public class RocksClearer : MonoBehaviour
    {
        public bool ClearOnThreshold = true;
        private List<Transform> rocks = new List<Transform>();
        private List<RockRigidbody> rbs = new List<RockRigidbody>();
        [SerializeField] private float gravMultiplyer_min, gravMultiplyer_max;
        [SerializeField] private float min_force, max_force;
        private List<Transform> ActiveShards;

        private void Start()
        {
            GameManager.Instance.eventManager.Stage_2_thresholdReached.AddListener(OnThresholdReached);
            GameManager.Instance.eventManager.GamePlay.AddListener(Init);
        }

        public void Init()
        {
            if (GameManager.Instance.data.currentLevelObjects == null)
                return;
            rocks = new List<Transform>();
            for(int i = 0; i < GameManager.Instance.data.currentLevelObjects.polishingObjects.SpawnedShardsRoot.transform.childCount; i++)
            {
                rocks.Add(GameManager.Instance.data.currentLevelObjects.polishingObjects.SpawnedShardsRoot.transform.GetChild(i));
            }
        }
        private void OnThresholdReached()
        {
            if (ClearOnThreshold == true)
            {
                GameManager.Instance.eventManager.ScoreAdded.AddListener(OnScoreAdded);
                GameManager.Instance.eventManager.StageFinished.AddListener(OnStageFinish);
                GetActiveShards();
            }
        }

        private void OnScoreAdded(int addedScore)
        {
            int clearCount = addedScore / GameManager.Instance.data.ScoreData.scoresData.RockPolishedScore;
            ClearActive(clearCount);
        }
        private void OnStageFinish()
        {
            GameManager.Instance.eventManager.StageFinished.RemoveListener(OnStageFinish);
            GameManager.Instance.eventManager.ScoreAdded.RemoveListener(OnScoreAdded);
        }
        private List<Transform> GetActiveShards()
        {
            ActiveShards = new List<Transform>();
            if (rocks == null)
                return null;

            foreach (Transform temp in rocks)
            {
                if (temp.gameObject.activeInHierarchy == true)
                {
                    ActiveShards.Add(temp);
                }
            }
            return ActiveShards;
        }
        private void ClearActive(int count)
        {
            if (ActiveShards == null) return;
            if (count > ActiveShards.Count)
            {
                count = ActiveShards.Count;
            }
            for(int i=0; i < count; i++)
            {
                ActiveShards[0].gameObject.SetActive(false);
                ActiveShards.Remove(ActiveShards[0]);
            }
        }
        public void ClearAll()
        {
            List<Transform> active = new List<Transform>();
            if (rocks == null)
                return; 
            foreach (Transform temp in rocks)
            {
                if (temp.gameObject.activeInHierarchy == true)
                {
                    active.Add(temp);
                    Rigidbody rigid = temp.gameObject.AddComponent<Rigidbody>();
                    float grabityMulpriler = Random.Range(gravMultiplyer_min, gravMultiplyer_max);
                    float force = Random.Range(min_force, max_force);
                    RockRigidbody rock = new RockRigidbody(rigid, Physics.gravity * grabityMulpriler, Random.onUnitSphere * force);
                    rock.Activate(ForceMode.Acceleration);
                }
            }
        }
    }



}