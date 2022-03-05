using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sculpturing;
using General;
using General.Data;
using Sculpturing.Levels;

public class PreGenTool : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private GameObject Level;
    private GameObject Stone;
    private GameObject Statue;
    private Transform Spawn;
    private Light Lights;
    private Transform GeometryCenter;
    [Header("Spawners")]
    [SerializeField] StoneSpawner stoneSpawner;
    [SerializeField] StatueSpawner statueSpawner;

    [Header("Stage 1 settings")]
    [SerializeField] private List<StonesShatterData> stage_1_settings = new List<StonesShatterData>();
    [Header("Stage 2 settings")]
    [SerializeField] private RockShardsSpawningData stage_2_settings;
    [Header("Lights")]
    private LevelObjects spawnedObjects;
    private LevelInstance currentLevel;
    public void PreGen()
    {
        Analyze();
        SoloGenStage1(false);
        SoloGenStage2(false);
    }
    public void SoloGenStage1(bool analyse)
    {
        if (analyse)
            Analyze();
        if (Stone == null)
            return;
        spawnedObjects = new LevelObjects();
        spawnedObjects.sculpturingObjects = new SculpturingData();
        spawnedObjects.polishingObjects = null;
        stoneSpawner.SpawnStone(Stone, stage_1_settings);
        spawnedObjects.sculpturingObjects.mainStone = Stone.GetComponent<StonePiece>();
        RecordCurrentData();
        stoneSpawner.Clear();
    }
    public void SoloGenStage2(bool analyse)
    {
        if (analyse)
            Analyze();
        if (Statue == null)
            return;
        spawnedObjects = new LevelObjects();
        spawnedObjects.sculpturingObjects = null;
        spawnedObjects.polishingObjects = new PolishingData();
        spawnedObjects.polishingObjects = statueSpawner.InitStatue(Statue, stage_2_settings);
        RecordCurrentData();
    }
    public void RecordCurrentData()
    {
        spawnedObjects.Statue = Statue;
        spawnedObjects.Stone = Stone;
        spawnedObjects.SpawnTransform = Spawn;
        spawnedObjects.LevelLight = Lights;
        spawnedObjects.GeometryCenter = GeometryCenter;
        Lights.enabled = false;
        currentLevel.RecordData(spawnedObjects);
        Debug.Log("<color=blue> All changes have been recorded to level script.Please save the Prefab.Record again if any changes are made </color>");
    }
    public void Analyze()
    {
        if (Level == null)
            Level = transform.parent.gameObject;
        Spawn = Helpers.FindByTag(Level.transform, Tags.Spawn);
        currentLevel = Level.transform.GetComponent<LevelInstance>();
        if(currentLevel == null)
        {
            Debug.Log("Level Instance Scripts not assigned, abort pre-gen");
            return;
        }
        Stone = Helpers.FindByTag(Spawn.GetChild(0), Tags.Stone).gameObject;
        Statue = Helpers.FindByTag(Spawn.GetChild(0), Tags.Statue).gameObject;
        Lights = Level.GetComponentInChildren<Light>();
        GeometryCenter = Spawn.GetChild(1);
    }
    public void DebugAnalyze()
    {
        if (Level == null)
            Level = transform.parent.gameObject;

        Spawn = Helpers.FindByTag(Level.transform, Tags.Spawn);
        currentLevel = Level.transform.GetComponent<LevelInstance>();
        if (currentLevel == null)
        {
            Debug.Log("Level Instance Scripts not assigned, abort pre-gen");
            return;
        }
        Stone = Helpers.FindByTag(Spawn.GetChild(0), Tags.Stone).gameObject;
        Statue = Helpers.FindByTag(Spawn.GetChild(0), Tags.Statue).gameObject;
        Lights = Level.GetComponentInChildren<Light>();
        GeometryCenter = Spawn.GetChild(1);
        statueSpawner.AnalyzeStatue(Statue);
    }
}
