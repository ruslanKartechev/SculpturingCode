using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Events;
using General;

namespace Sculpturing.UI
{
    public class GameUIController : MonoBehaviour
    {
        public StartPanel startPanel;
        public StageProgressPanel progressPanel;
        public LevelCompletePanel levelEndPanel;
        public void Init()
        {
            startPanel.Init();
            progressPanel.Init();
            levelEndPanel.Init();

            startPanel.HidePanel(true);
            progressPanel.HidePanel(true);
            levelEndPanel.HidePanel(true);

            GameManager.Instance.eventManager.LevelFinishInit.AddListener(OnLevelFinishedInit);
            GameManager.Instance.eventManager.CameraStartPositionSet.AddListener(OnCameraFirstSet);
            GameManager.Instance.eventManager.StartPanelHidden.AddListener(OnStartPanelHidden);
        }
        public void OnCameraFirstSet()
        {
            levelEndPanel.HidePanel(true);
            startPanel.ShowPanel();
        }

        public void OnStartPanelHidden()
        {
            progressPanel.ShowPanel();
        }

        public void OnLevelFinishedInit()
        {
            progressPanel.HidePanel(false);
        }

    }
}