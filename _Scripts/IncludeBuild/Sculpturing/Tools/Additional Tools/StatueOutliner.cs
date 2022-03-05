using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;
namespace Sculpturing.Tools
{

    public class StatueOutliner : MonoBehaviour
    {

        [SerializeField] private bool MakeOutline = true;
        [SerializeField] private Color mainColor;
        [SerializeField] private Color finishedColor;
        [SerializeField] private float outlineWidth;

        private GameObject currentStatue = null;
        private Outline currentOutliner = null;

        private void Start()
        {
            if (MakeOutline)
            {
                GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
                GameManager.Instance.eventManager.LevelFinishInit.AddListener(OnLevelFinishing);
            }
        }
        public void OnNewStage(StageData data)
        {
            if(data.stageNumber == 4)
            {
                currentStatue = GameManager.Instance.data.currentLevelObjects.Statue;
                if (currentStatue == null) return;
                currentOutliner = currentStatue.AddComponent<Outline>();
            }
        }
        private void OnLevelFinishing()
        {
            if (currentOutliner == null || MakeOutline == false)
                return;
            currentOutliner.enabled = true;
            InitOutline(finishedColor);
        }
        private void InitOutline(Color color)
        {
            if (currentOutliner == null)
                return;
            currentOutliner.OutlineWidth = outlineWidth;
            currentOutliner.OutlineColor = color;
        }

    }

}