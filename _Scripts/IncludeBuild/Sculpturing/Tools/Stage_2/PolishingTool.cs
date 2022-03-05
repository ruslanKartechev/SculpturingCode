using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;
namespace Sculpturing.Tools
{
    public class PolishingTool : Tool, ITool
    {
        [SerializeField] private LayerMask rocksMask;
        [SerializeField] private LayerMask statueMask;
        [SerializeField] private ScrabbingEffects effectsHandler;
        [SerializeField] private float RaycastRadius = 0.05f;
        [Tooltip("By how much multiply scores after threshold reached")]
        [SerializeField] private int scoresMultiplyer = 20;
        private bool thresholdReached = false;
        public void Init()
        {
            if (soundEffect == null)
                soundEffect = GetComponent<ISoundEffect>();
            HideTool();
            GameManager.Instance.eventManager.CameraPositionSet.AddListener(OnCameraSet);
            GameManager.Instance.eventManager.StageFinished.AddListener(OnStageFinished);
            if(WasInited == false)
            {
                WasInited = true;
                effectsHandler.PreInst();
            }
        }

        private void OnThresholdReached() => thresholdReached = true;


        private void Subscribe()
        {
            GameManager.Instance.eventManager.MouseDown.AddListener(Working);
            GameManager.Instance.eventManager.MouseUp.AddListener(Idle);
            GameManager.Instance.eventManager.Stage_2_thresholdReached.AddListener(OnThresholdReached);
            thresholdReached = false;
        }
        private void UnSubscribe()
        {
            GameManager.Instance.eventManager.MouseDown.RemoveListener(Working);
            GameManager.Instance.eventManager.MouseUp.RemoveListener(Idle);
            GameManager.Instance.eventManager.Stage_2_thresholdReached.RemoveListener(OnThresholdReached);
        }
        private void OnStageFinished()
        {
            GameManager.Instance.eventManager.StageFinished.RemoveListener(OnStageFinished);
            PlayIdleAmimation();
        }

        private void OnCameraSet()
        {
            ShowTool();
            GameManager.Instance.eventManager.CameraPositionSet.RemoveListener(OnCameraSet);
            GameManager.Instance.eventManager.ToolInited.Invoke(this);
            Subscribe();
        }
        public void OnSoundEffectEvent()
        {
            soundEffect.PlayEffectOnce();
            Vibration.VibratePop(); // vibration effect
        }
        public override void Working()
        {
            base.Working();
        }
        public override void Idle()
        {
            base.Idle();
        }
        public void OnAnimEvent()
        {
            if (FirstActionDone == false)
            {
                FirstActionDone = true;
                GameManager.Instance.eventManager.FirstToolAction.Invoke();
            }
            
            if (thresholdReached == true)
            {
                RaycastStatue();
                int score = scoresMultiplyer *
                    GameManager.Instance.data.ScoreData.scoresData.RockPolishedScore;
                GameManager.Instance.eventManager.ScoreAdded.Invoke(score);
                return;
            } else
                RaycastAndClear();

        }
        private void RaycastStatue()
        {
            Ray ray;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(actionPoint.position);
            ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit hit; 
            if(Physics.Raycast(ray, out hit, statueMask))
                effectsHandler.Emit(hit.point);
        }

        private void RaycastAndClear()
        {
            Ray ray = new Ray();
            Vector3 screenPos = Camera.main.WorldToScreenPoint(actionPoint.position);
            ray = Camera.main.ScreenPointToRay(screenPos);

            var hits = Physics.SphereCastAll(ray, RaycastRadius, 10, rocksMask);
            if (hits.Length > 0 )
            {
                effectsHandler.Emit(hits[0].point);
                foreach (RaycastHit hit in hits)
                {
                    hit.transform.gameObject.SetActive(false);
                    GameManager.Instance.eventManager.ScoreAdded.Invoke(
                        GameManager.Instance.data.ScoreData.scoresData.RockPolishedScore);
                }
            }
        }
        public void Disable()
        {
            UnSubscribe();
            Idle();
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
        private IEnumerator CheckTargets()
        {
            Ray ray;
            while (true)
            {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(actionPoint.position);
                    ray = Camera.main.ScreenPointToRay(screenPos);
                    var hits = Physics.SphereCastAll(ray, 0.2f, 10, ~rocksMask);
                    if (hits.Length > 0)
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            hit.transform.gameObject.SetActive(false);
                            GameManager.Instance.eventManager.ScoreAdded.Invoke(GameManager.Instance.data.ScoreData.scoresData.RockPolishedScore);
                             
                            yield return null;
                        }
                    }
                    else
                    {
                        yield return null;
                    }
            }
        }
    }
}