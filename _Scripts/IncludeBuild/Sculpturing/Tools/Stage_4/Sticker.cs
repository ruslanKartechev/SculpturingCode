using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;

namespace Sculpturing.Tools
{

    public class Sticker : MonoBehaviour, IClickable
    {
        private StickerPalet palet;
        private StickerData mSticker;
        [SerializeField] private Transform modelParent;
        private GameObject model;
        

        public void Init(StickerPalet _palet, StickerData mySticker)
        {
            palet = _palet;
            mSticker = mySticker;
            InitModel();
        }
        private void InitModel()
        {
            if (mSticker.model == null)
                return;
            if (modelParent.GetChild(0) != null)
                DestroyImmediate(modelParent.GetChild(0).gameObject);

            model = Instantiate(mSticker.model, modelParent);
            //model.transform.localScale = Vector3.one;
            //model.transform.localPosition = Vector3.zero;
        }
        public void OnClick()
        {
            palet.SetSticker(this,mSticker);
        }




        public void Disable()
        {
          //  Destroy(gameObject);
        }
    }
}
