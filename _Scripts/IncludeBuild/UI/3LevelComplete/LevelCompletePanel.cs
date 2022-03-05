using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.UI;
namespace Sculpturing.UI
{
    public class LevelCompletePanel : UIPanelManager
    {
        [SerializeField] private LevelCompletePanelUI panelUI;

        public void Init()
        {
            panelUI.Init(this);
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnNewLevel);
            mPanel = panelUI;
        }
        public void OnNewLevel()
        {
            int level = GameManager.Instance.levelManager.CurrentLevelIndex + 1;
            panelUI.SetLevel(level);
        }
        public override void OnMainButtonClick()
        {
            GameManager.Instance.eventManager.ClickableHit.Invoke();
            base.OnMainButtonClick();
            GameManager.Instance.eventManager.NextLevelCalled.Invoke();
        }
    }
}