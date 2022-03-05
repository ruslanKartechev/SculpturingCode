using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using General;
using GameAnalyticsSDK.Events;

public class SDKEventsHandler : MonoBehaviour
{

    private int currentLevel = 0;

    void Awake()
    {
        GameManager.Instance.eventManager.LevelLoaded.AddListener(OnLevelLoaded);
        GameManager.Instance.eventManager.LevelFinishInit.AddListener(OnLevelEndInit);
    }
    public void OnLevelLoaded()
    {
        currentLevel = GameManager.Instance.levelManager.CurrentLevelIndex;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevel.ToString());
    }
    private void OnLevelEndInit()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevel.ToString());
    }

}
