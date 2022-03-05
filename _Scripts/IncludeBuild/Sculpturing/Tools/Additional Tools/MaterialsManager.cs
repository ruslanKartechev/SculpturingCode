using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using General;
using General.Data;

namespace Sculpturing.Tools
{

    public class MaterialsManager : MonoBehaviour
    {

        private GameObject currentStatue;
        private float valueStart;
        [SerializeField] private bool AddNormals = false;
        [SerializeField] float ValueEnd = 0;
        [SerializeField] private string normalPropName;
        private Renderer mRenderer;
        [Header("Materials")]
        public Material mainStatueMaterial;
        public Material mainStoneMaterial;
        public Material stonePiecesMaterial;
        public Material stoneShardsMaterial;
        [Header("Colors")]
        public Color mainStatueColor;
        public Color mainStoneColor;
        public Color StonePiecesColor;
        public Color stoneShardsColor;
        [SerializeField] private string colorPropName;


        void Start()
        {
            GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnNewLevel);

        }

        private void OnNewLevel()
        {
            currentStatue = GameManager.Instance.data.currentLevelObjects.Statue;
            if (currentStatue == null) { Debug.Log("Statue was not found"); return; }
            if(AddNormals == false)
            {
                return;
            }
            mRenderer = currentStatue.GetComponent<Renderer>();
            valueStart = mRenderer.material.GetFloat(normalPropName);
        }
        private void OnNewStage(StageData data)
        {
            if (currentStatue == null || AddNormals == false) return;
            if(data.stageName == StageByName.Polishing)
            {
                GameManager.Instance.eventManager.ProgressAdded.AddListener(OnProgressAdded);
            }
            if(data.stageName == StageByName.Painting)
            {
                GameManager.Instance.eventManager.ProgressAdded.RemoveListener(OnProgressAdded);
            }

        }

        private void OnProgressAdded(float currentProgress)
        {
            float val = 0;
            if (currentProgress <= 1)
                val = Mathf.Lerp(valueStart,ValueEnd,currentProgress);
            mRenderer.material.SetFloat(normalPropName, val);
        }



        public void SetMaterialAndColor(GameObject target, string type)
        {
            Renderer rend = target.GetComponent<Renderer>();
            Material mat;
            switch (type)
            {
                case MaterialTypes.Statue:
                    rend.material = mainStatueMaterial;
                    mat = rend.material;
                    mat.SetColor(colorPropName, mainStatueColor);
                    break;
                case MaterialTypes.MainStone:
                    rend.material = mainStoneMaterial;
                    mat = rend.material;
                    mat.SetColor(colorPropName, mainStoneColor);
                    break;
                case MaterialTypes.StonePieces:
                    rend.material = stonePiecesMaterial;
                    mat = rend.material;
                    mat.SetColor(colorPropName, StonePiecesColor);
                    break;
                case MaterialTypes.StoneShards:
                    rend.material = stoneShardsMaterial;
                    mat = rend.material;
                    mat.SetColor(colorPropName, stoneShardsColor);
                    break;
                default:
                    Debug.Log("wrong type passed");
                    break;

            }
        }


    }
}