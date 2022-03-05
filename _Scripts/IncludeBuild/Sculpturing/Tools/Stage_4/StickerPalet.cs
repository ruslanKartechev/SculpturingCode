using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;
namespace Sculpturing.Tools
{
    public class StickerPalet : MonoBehaviour
    {
        [SerializeField] private List<Sticker> stickers = new List<Sticker>();
        
        [Header("Transform Settings")]
        [SerializeField] private float Zoffset;
        [SerializeField] private Vector2 ScreenOffset = new Vector2();
        [Header("Scaling settings")]
        public float CansOriginalScale = 1f;
        public float CansEnlargedScale = 1.2f;
        public float TimeToScale = 0.35f;

        private StickerPlacer mainSticker;
        private Sticker currentSticker;
        private bool firstActionDone = false;
        public void SetPosition()
        {
            transform.parent = Camera.main.transform.parent;
            Vector3 toolPos = new Vector3(Screen.width * ScreenOffset.x, Screen.height * ScreenOffset.y, Zoffset);
            transform.position = Camera.main.ScreenToWorldPoint(toolPos);
            transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
           // transform.rotation = Quaternion.identity;
            transform.localEulerAngles = new Vector3(GameManager.Instance.data.MainGameData.toolsData.StickersPaletXangle,
                    transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        public void SetStickers()
        {
            List<StickerData> stickerModels = GameManager.Instance.data.MainGameData.toolsData.defaultStickers;
            if (stickerModels.Count < stickers.Count) { Debug.Log("Not enough stickers provided in data"); return; }
            List<int> indeces = Helpers.GetRandomIndices(0, stickerModels.Count - 1, stickers.Count);

            for (int i = 0; i < indeces.Count; i++)
            {
                if (stickerModels[indeces[i]] != null)
                    stickers[i].Init(this, stickerModels[indeces[i]]);
                else
                    Debug.Log("Null model");
            }
        
        }


        public void SetSticker(Sticker sticker, StickerData data)
        {
            if(firstActionDone == false) { firstActionDone = true; }
            if (currentSticker != null)
            {
                StartCoroutine(Helpers.ChangeScale(currentSticker.transform, CansOriginalScale, TimeToScale));
            }
            currentSticker = sticker;
            StartCoroutine(Helpers.ChangeScale(currentSticker.transform, CansEnlargedScale, TimeToScale));

            mainSticker.SetSticker(data);
        }

        public void Init(StickerPlacer _mainSticker)
        {
            mainSticker = _mainSticker;
            // auto init the stickers;
            SetPosition();
            SetStickers();
        }
        public void Disable()
        {
            foreach (Sticker st in stickers)
            {
                st.transform.localScale = Vector3.one * CansOriginalScale;
            }
            gameObject.SetActive(false);
        }
    }
}