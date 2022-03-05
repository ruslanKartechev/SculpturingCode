using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.UI;
namespace Sculpturing.UI
{
    public class StartPanel : UIPanelManager
    {
        [SerializeField] private GameObject panelObj;
        [SerializeField] private StartPanelUI panelUI;


        public void Init()
        {
            mPanel = panelUI;
            panelUI.Init(this);
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnNewLevel);
        }
        public void OnNewLevel()
        {
            int level = GameManager.Instance.levelManager.CurrentLevelIndex + 1;
            panelUI.SetLevel(level);
        }
        public override void OnMainButtonClick()
        {
            GameManager.Instance.eventManager.ClickableHit.Invoke();
            HidePanel(true);
            GameManager.Instance.eventManager.GamePlay.Invoke();   
        }
        public override void OnPanelHidden()
        {
            base.OnPanelHidden();
            GameManager.Instance.eventManager.StartPanelHidden.Invoke();
        }

    }
}