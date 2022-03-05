using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sculpturing.Tools {
    [DisallowMultipleComponent]
    public class StickerClicker : MonoBehaviour, IClickable
    {
        public PlaceableSticker sticker;
        private Collider coll;
        public Collider Coll { get { return coll; } }
        public void Init(PlaceableSticker _sticker)
        {
            sticker = _sticker;
            SetCollider();
        }
        private void SetCollider()
        {
            if (coll == null)
            {
                coll = gameObject.AddComponent<BoxCollider>();
            }
        }
        public void OnClick()
        {
            if (sticker != null)
                sticker.OnClick();
        }

    }
}