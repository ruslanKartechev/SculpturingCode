using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using General.UI;
namespace Sculpturing.UI
{
    public class LevelCompletePanelUI : UIPanel
    {
        private LevelCompletePanel panel;
        public void Init(LevelCompletePanel _panel)
        {
            panelManager = _panel;
            panel = _panel;
            Init();
            mainButton.onClick.AddListener(_panel.OnMainButtonClick);
        }
        public void SetLevel(int CurrentLevel)
        {
            string leveltext = "Level " + CurrentLevel.ToString() + " Completed";
            SetHeaderText(leveltext);
        }
    }
}