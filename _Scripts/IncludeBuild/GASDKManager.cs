using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

public class GASDKManager : MonoBehaviour
{
    private void Awake()
    {
        GameAnalytics.Initialize();
    }
}
