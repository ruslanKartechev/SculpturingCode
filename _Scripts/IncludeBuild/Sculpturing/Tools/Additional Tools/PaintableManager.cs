using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using PaintIn3D;
using General.Data;
namespace Sculpturing.Tools
{
    public class PaintableManager : MonoBehaviour
    {
        [Header("Paintable settings")]
       // [SerializeField] private int statueLayer;
        [SerializeField] private Texture paintableTexture;
        [SerializeField] private Texture statueTexture;
        [SerializeField] private int copyMaterialIndex = 0;
        [SerializeField] private string textureReference;
        [SerializeField] private int textureSize = 256;
        private MeshRenderer mRenderer;
        // _BaseMap works for default shader
        private GameObject currentStatue;
        void Start()
        {
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnLevelLoaded);
            GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
        }

        private void OnLevelLoaded()
        {
            currentStatue = GameManager.Instance.data.currentLevelObjects.Statue;
            if (currentStatue == null) { Debug.Log("Statue was not found");return; }
            InitPaintable(currentStatue);   
        }
        private void InitPaintable(GameObject target)
        {
            ClearPaintable(target);
          //  target.layer = statueLayer;
            target.AddComponent<P3dPaintable>();
            P3dPaintableTexture paintTex = target.AddComponent<P3dPaintableTexture>();
            P3dMaterialCloner matCloner = target.AddComponent<P3dMaterialCloner>();
            matCloner.Index = copyMaterialIndex;
            paintTex.UndoRedo = P3dPaintableTexture.UndoRedoType.FullTextureCopy;
            paintTex.SaveLoad = P3dPaintableTexture.SaveLoadType.Manual;
            paintTex.Slot = new P3dSlot(0, textureReference);
            paintTex.Width = textureSize;
            paintTex.Height = textureSize;
            if (paintableTexture != null)
                paintTex.Texture = paintableTexture;
            else
                paintTex.Texture = statueTexture;
            paintTex.Activate();
            mRenderer = target.GetComponent<MeshRenderer>();
        }
        public void ClearPaintable(GameObject target)
        {
            if (target == null)
                return;
            P3dPaintable paintable = target.GetComponent<P3dPaintable>();
            P3dPaintableTexture paintTex = target.GetComponent<P3dPaintableTexture>();
            P3dMaterialCloner matCloner = target.GetComponent<P3dMaterialCloner>();
            if (matCloner != null)
                DestroyImmediate(matCloner);
            if (paintTex != null)
                DestroyImmediate(paintTex);
            if (paintable != null)
                DestroyImmediate(paintable);
        }

        public void OnNewStage(StageData data)
        {
            if(data.stageName == StageByName.Painting)
            {
                mRenderer.material.SetFloat("_NormalStrength", 0); 
            }
        }


    }
}