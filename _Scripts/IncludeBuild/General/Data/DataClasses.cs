using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sculpturing;
namespace General.Data
{
    [System.Serializable]
    public class SculpturingData
    {
        public StonePiece mainStone = null;
        public Transform Tier_1_root = null;
        public List<Transform> Tier_2_roots = new List<Transform>();
    }

    [System.Serializable]
    public class PolishingData
    {
        public GameObject SpawnedShardsRoot;
        public List<RockShardsData> shards = new List<RockShardsData>();
        public PolishingData() { }
        public PolishingData(PolishingData from)
        {
            SpawnedShardsRoot = from.SpawnedShardsRoot;
            shards = new List<RockShardsData>();
            for(int i = 0; i< from.shards.Count; i++)
            {
                shards.Add(new RockShardsData(from.shards[i]));
            }
        }
    }
    [System.Serializable]
    public class RockShardsData
    {
        public string path;
        public Vector3 localPos = new Vector3();
        public float localScale;
        public Vector3 localEulers = new Vector3();
        public RockShardsData() { }
        public RockShardsData(RockShardsData from)
        {
            path = from.path;
            localPos = from.localPos;
            localScale = from.localScale;
            localEulers = from.localEulers;
        }
    }

    [System.Serializable]
    public class LevelObjects
    {
        public GameObject Statue = null;
        public GameObject Stone = null;
        public Transform SpawnTransform = null;
        public Transform GeometryCenter = null;
        public Light LevelLight = null;
        public SculpturingData sculpturingObjects = null;
        public PolishingData polishingObjects = null;
        public LevelObjects()
        {

        }
        public void Record(LevelObjects from)
        {
            if(from.Statue!=null)
                Statue = from.Statue;
            if(from.Stone!=null)
                Stone = from.Stone;
            if(from.SpawnTransform != null)
                SpawnTransform = from.SpawnTransform;
            if(from.sculpturingObjects != null)
                sculpturingObjects = from.sculpturingObjects;
            if(from.polishingObjects != null)
                polishingObjects = from.polishingObjects;
            if (from.LevelLight != null)
                LevelLight = from.LevelLight;
            if (from.GeometryCenter != null)
                GeometryCenter = from.GeometryCenter;
        }

    }







    [System.Serializable]
    public class ScoreData
    {
        [Header("Stage 1 scores")]
        public int BigStoneBrokenScore;
        public int StonePieceBrokenScore;
        [Header("Stage 2 scores")]
        public int RockPolishedScore;
        [Header("Stage 4 max stickers count")]
        public int MaxStickersCount;
    }
    [System.Serializable]
    public class ThresholdData
    {
        [Header("Skip stage button appeargin threshold")]
        public int MainThreshold;
        [Header("Stage 2 auto-cleared threshhold")]
        public int Threshold_stage_2;
    }
    [System.Serializable]
    public class StoneBreakingData
    {
        public int Tier;
        public int HitsToCrack;
        public int HitsToBreak;
    }



    [System.Serializable]
    public class StageData
    {
        public int stageNumber;
        public List<string> subStagesHeaders = new List<string>();
        public string stageName;
        public GameObject stageTool;
    }

    [System.Serializable]
    public class LevelData
    {
        public GameObject lvlPF;
    }

    // data for stone shattering (two tiers)
    [System.Serializable]
    public class StonesShatterData
    {
        [Tooltip("0 - main stone, 1 - tier1 stones, tier2 stones don't shatter")]
        public int Tier;
        public int ShatterAmount;
        public float LocalScale;
        [Tooltip("voronoi piece min size in percent*100")]
        public int MinSize;
        public int HitsToCrack;
        public int HitsToBreak;
        public Material tierMaterial;
    }
    [System.Serializable]
    public class RockShardsSpawningData
    {
        [Tooltip("Small rocks prefab")]
        public List<GameObject> RockVariants = new List<GameObject>();
        [Header("Rocks Scale")]
        public bool variableScale = true;
        [Header("Fixed rock scale")]
        [Range(0f, 1f)]
        public float fixedScale = 0.5f;
        [Header("Variable scale")]
        [Range(0f, 1f)]
        public float scale_min = 0.5f;
        [Range(0f, 2f)]
        public float scale_max = 1.5f;
        public int spacing = 15;
        public int rocksLayer = 10;
    }


    [System.Serializable]
    public class StickerData
    {
        public string name;
        public GameObject model;
    }

    // data for spawning tools
    [System.Serializable]
    public class ToolsData
    {
        [Tooltip("Color palet X euler Angle")]
        public float ColorPaletXangle;
        [Tooltip("Sticker palet X euler Angle")]
        public float StickersPaletXangle;
        [Header("Individual settings for each tool")]

        public Vector3 PaintingOffsetVector = new Vector3();
        [Header("Deafult colors available for color palet")]
        public List<Color> defaultPaletColors = new List<Color>();
        [Header("Default stickers")]
        public List<StickerData> defaultStickers = new List<StickerData>();
    }




    [System.Serializable]
    public class ToolMoverData
    {
        public float RelativeLimit_hammer;
        public float RelativeLimit_polishing;
        public float RelativeLimit_painting;
    }

    [System.Serializable]
    public class FinishingSequenceData
    {
        public float StatueBackRotationTime = 1f;
        public float CameraBackMovementTime = 1.2f;
        [Header("Delays")]
        public float RotationStartDelay = 0f;
        public float LevelCompletePanelShowDelay = 0f;
        public float NextLevelButtonShowDelay = 1f;
    }

    [System.Serializable]
    public class StonePieceLoadData
    {
        public List<string> piecesPaths = new List<string>();
        public StonePieceLoadData() 
        {
            piecesPaths = new List<string>();
        }


    }
    [System.Serializable]
    public class StonePieceData
    {
        public int Tier;
        public string MeshPath;
        public Vector3 localPos = new Vector3();
        public Vector3 localEulers = new Vector3();
        public float localScale;


    }

    public class TempStageData
    {
        [Tooltip("How many pieces are spawned in stage one")]
        public int StageOneSpawned;
        [Tooltip("How many pieces are spawned in stage two")]
        public int StageTwoSpawned;

        public TempStageData(int stageOne, int stageTwo)
        {
            StageOneSpawned = stageOne;
            StageTwoSpawned = stageTwo;
        }
    }



}