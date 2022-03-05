using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using PaintIn3D;
namespace Sculpturing
{
    public class StatueSpawner : MonoBehaviour
    {
        [Header("Textures")]
        [SerializeField] private Texture2D statueTexture;
        [SerializeField] private Material material;
        [Space(10)]
        [SerializeField] private int StatueLayer = 9;
        [Space(10)]
        [SerializeField] private VertexRockSpawner rocksSpawner;
        [HideInInspector] public GameObject mainStatue { get; private set; }
        public PolishingData SpawnedData { get; private set; }
        private void InitTextures(GameObject target)
        {
            Renderer rend = target.GetComponent<Renderer>();
            rend.material = material;
            material.SetTexture( "_MainTex", statueTexture);
        }
        private PolishingData SpawnRocks(GameObject target, RockShardsSpawningData data)
        {
            mainStatue = target;
            rocksSpawner.Init(target, data);
            return rocksSpawner.SpawnRocks();
        }

        public PolishingData InitStatue(GameObject model, RockShardsSpawningData data)
        {
            model.layer = StatueLayer;
            if (rocksSpawner == null)
                rocksSpawner = GetComponent<VertexRockSpawner>();
            InitTextures(model);
            SpawnedData = new PolishingData(SpawnRocks(model, data));
            rocksSpawner.ClearSpawned();
            return SpawnedData;
        }

        public void AnalyzeStatue(GameObject statue)
        {
            if (rocksSpawner == null)
                rocksSpawner = GetComponent<VertexRockSpawner>();
            rocksSpawner.AnalyzeVertices(statue);
        }

    }
}