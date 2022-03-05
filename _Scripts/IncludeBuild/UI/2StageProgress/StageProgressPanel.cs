using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;
using General.UI;
namespace Sculpturing.UI
{
    public class StageProgressPanel : UIPanelManager
    {
        [SerializeField] private StageProgressPanelUI panelUI;
        private bool IsEditor;
        public void Init()
        {
            mPanel = panelUI;
            panelUI.Init(this);
            IsEditor = GameManager.Instance.data.MainGameData.EditorUIMode;
            if(IsEditor == false)
            {
                GameManager.Instance.eventManager.FirstToolAction.AddListener(OnFirstActionDone);
                GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
                GameManager.Instance.eventManager.ProgressAdded.AddListener(OnProgress);
                GameManager.Instance.eventManager.StageFinished.AddListener(OnStageFinish);
            }
            else
            {

            }

        }

        public override void ShowPanel(bool showButton = true)
        {
            if(IsEditor == false)
            {
                base.ShowPanel(showButton);
            }
            else
            {
                panelUI.gameObject.SetActive(true);
                panelUI.ShowEditorPanel();
            }

        }


        public void OnNewStage(StageData data)
        {
            panelUI.SetProgressUI(0);
            panelUI.StartHeaderAnimator();
            mHeaders = data.subStagesHeaders;
            if(data.subStagesHeaders.Count > 0)
            {
                currentHeader = 0;
                panelUI.SetHeaderText(data.subStagesHeaders[currentHeader]);
            }
            if (data.stageNumber == 3)
                panelUI.HidePorgressBar();
        
        }
        public override void SwitchHeader(int dir)
        {
            base.SwitchHeader(dir);
            panelUI.StartHeaderAnimator();
        }
        private void OnStageFinish()
        {
          //  panelUI.SetProgressUI(0);
        }
        private void OnFirstActionDone()
        {
            mPanel.StopHeaderAnimator();
        }
        private void OnProgress(float progress)
        {
            panelUI.SetProgressUI(progress);
        }


    }
}