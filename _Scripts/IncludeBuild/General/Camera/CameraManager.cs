using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using General;
namespace Sculpturing.Cam
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private bool DoShakeCamera = false;
        [SerializeField] private CameraMovement movementHandler;
        [SerializeField] private CameraShake shakingHandler;
        [SerializeField] private Transform Tartget;
        private void Awake()
        {
            if (GameManager.Instance.mainCam == null)
                GameManager.Instance.mainCam = this;
        }
        private void Start()
        {
            if (Tartget == null)
                Tartget = transform.parent;
            movementHandler.Init(this, Tartget);
            shakingHandler.Init(this, Tartget);
            GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnNewLevel);
            if(DoShakeCamera == true)
                GameManager.Instance.eventManager.Impact.AddListener(OnImpact);
            GameManager.Instance.eventManager.GamePlay.AddListener(movementHandler.OnGamePlay);
        }
        private void OnNewLevel()
        {
            movementHandler.OnLevelLoaded();
        }
        private void OnNewStage(StageData data)
        {
            movementHandler.OnStageChange(data);
        }
        private void OnImpact(int magnitude)
        {
            shakingHandler.OnImpact(magnitude);
        }
        public void ReturnToStartPos()
        {
            movementHandler.GoToLevelEndPos();
        }
    }

}