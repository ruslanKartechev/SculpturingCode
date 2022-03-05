using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Events;
using General.Sound;
using General.Data;
using Sculpturing.Levels;
using Sculpturing.UI;
using Sculpturing.Cam;
using Sculpturing.Tools;
namespace General
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : SingletonMB<GameManager>
    {
        [Header("Debugging")]
        public bool DoStartGame = true;
        public LevelManager levelManager;
        public SoundEffectManager soundManager;
        public EventsManager eventManager;
        public DataManager data;
        public DataLoader dataLoader;
        public Controlls controlls;
        public GameUIController gameUIController;
        public ToolsManager toolsManager;
        public CameraManager mainCam;
        [SerializeField] private StageManager stageManager;

        private void Start()
        {
            eventManager.DataLoaded.AddListener(OnDataLoaded);
            dataLoader.StartLoading();
        }
        private void OnDataLoaded()
        {
            StartCoroutine(InitRoutine());
        }

        private IEnumerator InitRoutine()
        {
            controlls.Init();
            soundManager.Init();
            yield return null;
            stageManager.Init();
            toolsManager.Init();
            yield return null;
            gameUIController.Init();
            yield return null;
            StartGame();
        }

        public void StartGame()
        {
            levelManager.PlayLastLevel();
        }
    }
}
    public class SingletonMB<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<T>();
                return instance;
            }
            set
            {
                instance = value;
            }
        }
    }
