using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;
namespace Sculpturing.Tools
{
    public class HammerTool : Tool, ITool
    {
        [SerializeField] private HammerEffects effectsHandler;
        [SerializeField] private LayerMask mask;
        public void Init()
        {
            HideTool();
            if (actionPoint == null) { Debug.Log("Action point not assigned"); return; }
            GameManager.Instance.eventManager.CameraPositionSet.AddListener(OnCameraSet);
            if (effectsHandler == null)
                effectsHandler = GetComponent<HammerEffects>();
            if (WasInited == false)
            {
                WasInited = true;
                effectsHandler.PreInst();
            }
        }
        private void SubScribe()
        {
            GameManager.Instance.eventManager.MouseDown.AddListener(Working);
            GameManager.Instance.eventManager.MouseUp.AddListener(Idle);
        }
        private void UnSubscribe()
        {
            GameManager.Instance.eventManager.MouseDown.RemoveListener(Working);
            GameManager.Instance.eventManager.MouseUp.RemoveListener(Idle);
        }
        private void OnCameraSet()
        {
            GameManager.Instance.eventManager.CameraPositionSet.RemoveListener(OnCameraSet);
            ShowTool();
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            GameManager.Instance.eventManager.ToolInited.Invoke(this);
            SubScribe();
        }


        public override void Working()
        {
            base.Working();
            if (toolRotatable.activeInHierarchy == false)
                toolRotatable.SetActive(true);
            PlayActiveAnimation();
        }
        public override void Idle()
        {
            base.Idle();
        }
        public void Disable()
        {
            StopAllCoroutines();
            PlayIdleAmimation();
            UnSubscribe();
            gameObject.SetActive(false);
        }
        public void Hit() // called by animation event
        {
            if(FirstActionDone == false)
            {
                FirstActionDone = true;
                GameManager.Instance.eventManager.FirstToolAction.Invoke();
            }
            Vector3 screenPos = Camera.main.WorldToScreenPoint(actionPoint.position);
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit[] hits;            
            
            hits = Physics.RaycastAll(ray, 20, mask);
            if (hits.Length > 0)
            {
                GameManager.Instance.eventManager.Impact.Invoke(1);
                if (soundEffect != null)
                    soundEffect.PlayEffectOnce();
                Vibration.VibratePop();
                foreach (RaycastHit hit in hits)
                {
                    hit.collider.gameObject.GetComponent<IToolTarget>().TakeHit();
                }
                
                
            }                 

        }
     

    }

}