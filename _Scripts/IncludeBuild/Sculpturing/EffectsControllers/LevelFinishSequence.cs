using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using Sculpturing.Tools;
using General.Data;
namespace Sculpturing
{
    public class LevelFinishSequence : MonoBehaviour
    {

        [SerializeField] private Vector3 confettiCameraOffet = new Vector3();
        [SerializeField] private GameObject confettiPF;
        [SerializeField] private GameObject shiningPF;
        private Animator shiningAnimator;
        [SerializeField] private string shiningAnimationName;

        private float StatueRotationTime;
        private float CameraMovementTime;
        private float rotationStartDelay;
        private float panelShowDelay;
        private float buttonShowDelay;

        private GameObject confettiInst;
        private GameObject shiningInst;

        private StatueRotator statueRotator;
        private LevelObjects currentObjects;
        private bool nextLevelLoaded = false;

        private MySoundPlayer soundPlayer;
        private void Start()
        {
            if (soundPlayer == null)
                soundPlayer = GetComponent<MySoundPlayer>();
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnNewLevel);
            GameManager.Instance.eventManager.LevelFinishInit.AddListener(OnLevelFinishInit);
            GameManager.Instance.eventManager.NextLevelCalled.AddListener(OnNextLevelInit);
            GameManager.Instance.eventManager.HiddenLoadingComplete.AddListener(OnHiddenLoadingComplete);
            StatueRotationTime = GameManager.Instance.data.FinishingSequence.StatueBackRotationTime;
            CameraMovementTime = GameManager.Instance.data.FinishingSequence.CameraBackMovementTime;
            rotationStartDelay = GameManager.Instance.data.FinishingSequence.RotationStartDelay;
            panelShowDelay = GameManager.Instance.data.FinishingSequence.LevelCompletePanelShowDelay;
            buttonShowDelay = GameManager.Instance.data.FinishingSequence.NextLevelButtonShowDelay;
            statueRotator = transform.parent.GetComponentInChildren<StatueRotator>();
        }
        /// <summary>
        /// Sequence:
        /// Clear tools
        /// ShootConfetti
        /// Small delay, if any
        /// Rotate to face Camera
        /// Delay untill statue rotating
        /// Set shining sprite, call Camera to move back
        /// Delay until Camera moves back
        /// Show level finish panel
        /// delay until showing button
        /// show button
        /// </summary>
       
        private void OnNewLevel()
        {
            currentObjects = GameManager.Instance.data.currentLevelObjects;
        }
        private IEnumerator ShowEffects()
        {
            //GameManager.Instance.levelManager.NextLevel(); // new addition
            GameManager.Instance.toolsManager.Clear();
            ShootConfettin();
            yield return new WaitForSeconds(rotationStartDelay);
            SetShining();
            soundPlayer.PlayEffectOnce();
            statueRotator.OnLevelFinish();
            GameManager.Instance.mainCam.ReturnToStartPos();
            GameManager.Instance.eventManager.LevelFinised.Invoke();
            yield return new WaitForSeconds(CameraMovementTime);
            while(nextLevelLoaded == false)
            {
                yield return null;
            }
            GameManager.Instance.gameUIController.levelEndPanel.ShowPanel(true);
        }
        private void OnHiddenLoadingComplete()
        {
            nextLevelLoaded = true;
        }

        private void OnNextLevelInit()
        {
            HideShining();
            HideConfetti();
            nextLevelLoaded = false;
            GameManager.Instance.levelManager.ShowNextLevel();
        }
        private void OnLevelFinishInit()
        {
            GameManager.Instance.controlls.StopInput();
            StartCoroutine(ShowEffects());
        }
        private void ShootConfettin()
        {
            if (confettiInst == null)
            {
                confettiInst = Instantiate(confettiPF);
                confettiInst.transform.position = Camera.main.transform.position + confettiCameraOffet;
            }
            else
            {
                confettiInst.transform.position = Camera.main.transform.position + confettiCameraOffet;
                confettiInst.SetActive(true);
            }
        }

        private void SetShining()
        {
            GameManager.Instance.eventManager.CameraMovementStarted.AddListener(OnCameraMovementStart);
        }
        private void OnCameraMovementStart(RotationData data)
        {
            GameManager.Instance.eventManager.CameraMovementStarted.RemoveListener(OnCameraMovementStart);
            Transform center = currentObjects.SpawnTransform.GetChild(1).transform;
            center.localPosition = new Vector3(0,center.localPosition.y,0);
          //  center.position = center.position - (data.target.position - center.position).normalized;
            center.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

            if (shiningInst == null)
                shiningInst = Instantiate(shiningPF, center);
            else
                shiningInst.SetActive(true);
            shiningInst.transform.parent = center;
            shiningInst.transform.position = center.position;
            if (shiningAnimator == null)
                shiningAnimator = shiningInst.GetComponent<Animator>();
            shiningAnimator.Play(shiningAnimationName, 0, 0);
            StartCoroutine(ShiningRotationHandler(center,data.target,data.rotationTime));
        }
        public IEnumerator ShiningRotationHandler(Transform target, Transform reference, float time)
        {
            Quaternion from = target.rotation;
            Quaternion to = Quaternion.LookRotation(reference.forward);
            float timeElapsed = 0;
            Vector3 dir = (reference.position - target.position).normalized;
            Vector3 startPos = target.position;
            target.position = startPos - dir;
            while (timeElapsed <= time)
            {
                dir = (reference.position - target.position).normalized;
                target.position = startPos - dir;
                target.rotation = Quaternion.Lerp(from, to, timeElapsed / time);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            target.position = target.position - dir;
            target.rotation = to;

        }
        private void HideConfetti()
        {
            if (confettiInst != null)
            {
                confettiInst.transform.parent = null;
                confettiInst.SetActive(false);
            }

        }
        private void HideShining()
        {
            if (shiningInst != null)
            {
                shiningInst.transform.parent = null;
                shiningInst.SetActive(false);
            }

        }


    }



}