using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;

namespace Sculpturing.Tools
{

    public class StickerPlacer : Tool, ITool
    {
        [SerializeField] private GameObject PaletPF;
        [SerializeField] private Transform modelParent;
        [SerializeField] private LayerMask statueMask;
        [SerializeField] private float rayRadius;
        [SerializeField] private float stickersScale = 10;
        [Tooltip("Local scale modifier, while moving sticker in hand")]
        [SerializeField] private float ScaleModifyier = 0.8f;
        [Header("Layers")]
        [SerializeField] private int holdLayer;
        [SerializeField] private int placedLayer;
        [SerializeField] private bool AutoPlace = true;
        private GameObject currentStickerModel;
        private GameObject paletObj;
        private StickerPalet palet = null;
        private PlaceableSticker currentSticker;

        private bool callScore = true;
        private IToolEffect myEffects;
        private MySoundPlayer myPlayer;

        public void Init()
        {
            if (myEffects == null)
                myEffects = GetComponent<IToolEffect>();
            if (myPlayer == null)
                myPlayer = GetComponent<MySoundPlayer>();
            if (paletObj == null)
            {
                paletObj = Instantiate(PaletPF, Camera.main.transform.parent);
            }
            else
            {
                paletObj.SetActive(true);
            }
            if (palet == null)
                palet = paletObj.GetComponent<StickerPalet>();
            palet.gameObject.SetActive(true);
            palet.Init(this);
            GameManager.Instance.eventManager.ToolInited.Invoke(this);
            modelParent.localScale = Vector3.one* stickersScale;
        }
        public void SetSticker(StickerData stickerData)
        {
            if (AutoPlace)
            {
                if(currentSticker != null)
                {
                    CheckTarget();
                }
            }
            else
            {
                ClearModel();
            }
            callScore = true;
            currentSticker = null;
            mover.SetToolCenterPosition();
            currentStickerModel = Instantiate(stickerData.model, modelParent);
            SetStickerModel(currentStickerModel);
            currentSticker = currentStickerModel.AddComponent<PlaceableSticker>();
            currentSticker.Init(this);
            GameManager.Instance.gameUIController.progressPanel.SwitchHeader(1);
            GameManager.Instance.eventManager.MouseDown.AddListener(OnMouseClick);
        }
        public void PickUpSticker(PlaceableSticker sticker)
        {
            mover.SetToolToMousePosition();
            if (currentStickerModel != null)
                return;
            currentSticker = sticker;
            currentStickerModel = sticker.gameObject;
            currentSticker.SetLayer(holdLayer);
            callScore = false;
            currentStickerModel.transform.parent = modelParent.transform;
            SetStickerModel(currentStickerModel);
            GameManager.Instance.gameUIController.progressPanel.SwitchHeader(1);
            GameManager.Instance.eventManager.MouseDown.Invoke();
            GameManager.Instance.eventManager.MouseUp.AddListener(OnMouseRelease);
        }
        private void SetStickerModel(GameObject target)
        {
            target.transform.localPosition = Vector3.zero;
            target.transform.localScale = Vector3.one* ScaleModifyier;
            target.transform.rotation = Quaternion.LookRotation(-transform.forward);
           // currentStickerModel.layer = holdLayer;
        }
        private void OnMouseClick()
        {
            GameManager.Instance.eventManager.MouseDown.RemoveListener(OnMouseClick);
            GameManager.Instance.eventManager.MouseUp.AddListener(OnMouseRelease);
        }
        private void OnMouseRelease()
        {
            CheckTarget();
            GameManager.Instance.eventManager.MouseUp.RemoveListener(OnMouseRelease);
        }
        private void CheckTarget()
        {
            if (currentStickerModel == null)
                return;
            Ray ray = new Ray();
            ray.origin = transform.position;
            ray.direction = transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10, statueMask))
            {
                GameManager.Instance.gameUIController.progressPanel.SwitchHeader(-1);
                if (currentSticker != null)
                {
                    currentSticker.Place(hit);
                    currentSticker.SetLayer(placedLayer);
                    if(myEffects != null)
                    {
                        myEffects.SetEffectPosRot(hit.point, Quaternion.LookRotation(hit.normal));
                        myEffects.PlayEffect();
                    }
                    if(myPlayer != null)
                    {
                        myPlayer.PlayEffectOnce();
                    }
                }
                currentStickerModel = null;
                if(callScore)
                    GameManager.Instance.eventManager.ScoreAdded.Invoke(1);
            }
            else
            {
                Debug.Log("Did not hit target");
            }
        }
        public void ClearModel()
        {
            if (currentStickerModel != null)
                Destroy(currentStickerModel);
        }
        public void Disable()
        {
            if (currentSticker != null)
            {
                CheckTarget();
            }
            ClearModel();
            palet.Disable();
            gameObject.SetActive(false);
        }

    }
}
