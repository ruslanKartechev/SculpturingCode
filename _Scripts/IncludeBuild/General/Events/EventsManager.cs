using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using General.Data;
using Sculpturing.Tools;
namespace General.Events
{
 
    public class ArgumentedEvent<T> : UnityEvent<T>
    {

    }

    public class EventsManager : MonoBehaviour
    {
        public UnityEvent DataLoaded = new UnityEvent();      // loading routing finished
        public UnityEvent GamePlay = new UnityEvent();       // start button pressed

        public UnityEvent MouseDown = new UnityEvent();
        public UnityEvent MouseUp = new UnityEvent();
        public UnityEvent ClickableHit = new UnityEvent(); // is also called when UI buttons are clicked;
        

        public UnityEvent InputStopped = new UnityEvent();
        public UnityEvent InputResumed = new UnityEvent();
        // Levels
        public UnityEvent HiddenLoadingComplete = new UnityEvent();
        public UnityEvent NextLevelCalled = new UnityEvent();
        public UnityEvent LevelLoaded = new UnityEvent();
        public UnityEvent LevelFinishInit = new UnityEvent();
        public UnityEvent LevelFinised = new UnityEvent(); // called to clear tools 
        // UI
        public UnityEvent CameraStartPositionSet = new UnityEvent(); // called to init startPanel
        public UnityEvent StartPanelHidden = new UnityEvent();
        // Stages
        public ArgumentedEvent<StageData> NewStageLoaded = new ArgumentedEvent<StageData>(); // called by manager
        public ArgumentedEvent<Tool> ToolInited = new ArgumentedEvent<Tool>();
        public UnityEvent StageFinished = new UnityEvent(); // called by progress or ui
        public UnityEvent FirstSelectionMade = new UnityEvent(); // called when color is chosen first time in the scene
        // Camera
        public ArgumentedEvent<RotationData> CameraMovementStarted = new ArgumentedEvent<RotationData>();
        public UnityEvent CameraPositionSet = new UnityEvent();
        public ArgumentedEvent<int> Impact = new ArgumentedEvent<int>();
        // Tools
        public UnityEvent FirstToolAction = new UnityEvent();
        public UnityEvent StoneBroken = new UnityEvent();
        public ArgumentedEvent<int> ScoreAdded = new ArgumentedEvent<int>();
        public ArgumentedEvent<float> ProgressAdded = new ArgumentedEvent<float>();
        public UnityEvent ThresholdReached = new UnityEvent();
        public UnityEvent Stage_2_thresholdReached = new UnityEvent();
    }
}