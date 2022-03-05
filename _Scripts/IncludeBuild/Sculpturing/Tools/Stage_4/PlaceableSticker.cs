using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;

namespace Sculpturing.Tools
{
    [DisallowMultipleComponent]
    public class PlaceableSticker : MonoBehaviour
    {
        private Collider col = null;
        private StickerPlacer placer = null;
     //   private StickerData mData = null;
        private bool IsPlaced = false;
        private StickerClicker myClicker;
        private Transform myModel;
        private bool clickerSet = false;
        public void Init(StickerPlacer _placer)
        {
            IsPlaced = false;
            placer = _placer;
            if (myModel != null)
                myModel.gameObject.layer = gameObject.layer;
            if(myClicker == null)
            {
                SetClicker();
            }
        }
        private void SetClicker()
        {
            if (myModel == null) { myModel = transform.GetChild(0); }
            if (myModel != null)
            {
                if (myClicker == null)
                {
                    myClicker = myModel.gameObject.GetComponent<StickerClicker>();
                    if (myClicker == null)
                    {
                        myClicker = myModel.gameObject.AddComponent<StickerClicker>();
                    }
                    myClicker.Init(this);
                }
            }
            else
                Debug.Log("Sticker model child not found");
        }
        public void SetLayer(int layer)
        {
            gameObject.layer = layer;
            myModel.gameObject.layer = layer;
        }

        public void Place(RaycastHit hit)
        {
            IsPlaced = true;
            transform.parent = hit.transform;
            transform.position = hit.point;
            transform.rotation = Quaternion.LookRotation(hit.normal);

        }

        public void OnClick()
        {
            if(IsPlaced == true)
            {
                placer.PickUpSticker(this);
                IsPlaced = false;
            }
               
        }
    }
}