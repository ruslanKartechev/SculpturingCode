using System.Collections.Generic;
using UnityEngine;
using General.Data;
using General;
using Sculpturing.Tools;
using System.Collections;
namespace Sculpturing.Levels 
{
    public class LevelLoader : MonoBehaviour
    {
        private LevelManager manager;
        public Transform levelPoint;
        [SerializeField] private StageManager stagesManager;
        [HideInInspector] public GameObject CurrentLevelObj;
        [Header("Settings")]
        [SerializeField] private int spawningSpacing = 20;
        [SerializeField] private Vector3 hiddenPosition = new Vector3();
        [SerializeField] private int ShardsLayer = 10;

        private GameObject oldLevel = null;
        private bool firstLevel = true;
        private Light newLevelLight;
        public void Init(LevelManager _manager)
        {
            manager = _manager;
        }
        public void Load(LevelData data)
        {
            if (oldLevel == null)
                ClearLevel();
            LoadSilent(data);
        }
        public void ShowLoadedLevel()
        {
            ClearOld();
            CurrentLevelObj.transform.localPosition = Vector3.zero;
            if(newLevelLight != null)
            {
                newLevelLight.gameObject.SetActive(true);
                newLevelLight.enabled = true;
            }
            GameManager.Instance.eventManager.LevelLoaded.Invoke();
        }
        public void LoadSilent(LevelData data)
        {
            CurrentLevelObj = Instantiate(data.lvlPF, levelPoint);
            CurrentLevelObj.transform.localPosition = hiddenPosition;
            LevelInstance currentLevel = CurrentLevelObj.GetComponent<LevelInstance>();
            LevelObjects levelObj = currentLevel.MyLevelObjects;
            GameManager.Instance.data.currentLevel = currentLevel;
            newLevelLight = levelObj.LevelLight;
            newLevelLight.enabled = false;
            GameManager.Instance.data.currentLevelObjects = levelObj;
            StartCoroutine(SilentLoading(levelObj));
        }

        public void LoadImmidiate(LevelData data)
        {
            CurrentLevelObj = Instantiate(data.lvlPF, levelPoint);
            LevelInstance currentLevel = CurrentLevelObj.GetComponent<LevelInstance>();
            LevelObjects levelObj = currentLevel.MyLevelObjects;
            GameManager.Instance.data.currentLevel = currentLevel;
            newLevelLight = levelObj.LevelLight;
            newLevelLight.enabled = true;
            GameManager.Instance.data.currentLevelObjects = levelObj;
            StonePiece main = levelObj.sculpturingObjects.mainStone;
            if (main == null)
                main = levelObj.Stone.GetComponent<StonePiece>();
            main.Init();
            main.LoadChildren(LoadedNotification,false);
            levelObj.sculpturingObjects.Tier_1_root = main.PiecesRoot;
            for (int i = 0; i < main.ChildPieces.Count; i++)
            {
                main.ChildPieces[i].LoadChildren(()=> { }, false);
                levelObj.sculpturingObjects.Tier_2_roots.Add(main.ChildPieces[i].PiecesRoot);
            }
            Transform shardsRoot = new GameObject("ShardsRoot").transform;
            shardsRoot.parent = levelObj.Statue.transform;
            shardsRoot.localEulerAngles = Vector3.zero;
            shardsRoot.localScale = Vector3.one;
            shardsRoot.localPosition = Vector3.zero;

            levelObj.polishingObjects.SpawnedShardsRoot = shardsRoot.gameObject;
            List<RockShardsData> shardsData = levelObj.polishingObjects.shards;
            for (int i = 0; i < shardsData.Count; i++)
            {
                GameObject temp = (GameObject)Resources.Load(shardsData[i].path);
                temp = Instantiate(temp, shardsRoot);
                temp.transform.localPosition = shardsData[i].localPos;
                temp.transform.localEulerAngles = shardsData[i].localEulers;
                temp.transform.localScale = shardsData[i].localScale * Vector3.one;
            }
            shardsRoot.transform.parent = levelObj.Statue.transform.parent;
            ClearOld();
            oldLevel = CurrentLevelObj;
            GameManager.Instance.eventManager.LevelLoaded.Invoke();
        }

        bool currentLoaded = false;
        public IEnumerator SilentLoading(LevelObjects levelObj)
        {
            Debug.Log("Starter hidden loading");
            MaterialsManager matManager = GameManager.Instance.toolsManager.materialsManager;
            matManager.SetMaterialAndColor(levelObj.Stone, MaterialTypes.MainStone);
            matManager.SetMaterialAndColor(levelObj.Statue, MaterialTypes.Statue);
            yield return null;
            StonePiece main = levelObj.sculpturingObjects.mainStone;
            if (main == null)
                main = levelObj.Stone.GetComponent<StonePiece>();
            if (main.gameObject.activeInHierarchy == false)
                main.gameObject.SetActive(true);
            matManager.SetMaterialAndColor(main.gameObject, MaterialTypes.MainStone);
            main.Init();
            main.LoadChildren(LoadedNotification, true);
            while (currentLoaded == false)
            {
                yield return null;
            }
            currentLoaded = false;
            levelObj.sculpturingObjects.Tier_1_root = main.PiecesRoot;
            for(int i=0; i < main.ChildPieces.Count; i++)
            {
                currentLoaded = false;
                main.ChildPieces[i].LoadChildren(LoadedNotification, true);

                while (currentLoaded == false)
                {
                    yield return null;
                }
                levelObj.sculpturingObjects.Tier_2_roots.Add(main.ChildPieces[i].PiecesRoot);
            }

            Transform shardsRoot = new GameObject("ShardsRoot").transform;
            shardsRoot.parent = levelObj.Statue.transform;
            shardsRoot.localEulerAngles = Vector3.zero;
            shardsRoot.localScale = Vector3.one;
            shardsRoot.localPosition = Vector3.zero ;
            
            levelObj.polishingObjects.SpawnedShardsRoot = shardsRoot.gameObject;
            List<RockShardsData> shardsData = levelObj.polishingObjects.shards;
            for(int i = 0; i < shardsData.Count; i++)
            {

                GameObject temp = (GameObject)Resources.Load(shardsData[i].path);
                temp = Instantiate(temp, shardsRoot);
                matManager.SetMaterialAndColor(temp, MaterialTypes.StoneShards);
                temp.transform.localPosition = shardsData[i].localPos;
                temp.transform.localEulerAngles = shardsData[i].localEulers;
                temp.transform.localScale = shardsData[i].localScale * Vector3.one;
                temp.layer = ShardsLayer;
                temp.AddComponent<SphereCollider>();
                if (i % spawningSpacing == 0)
                    yield return null;
            }
            shardsRoot.transform.parent = levelObj.Statue.transform.parent;
            yield return null;
            if(firstLevel == true)
            {
                ShowLoadedLevel();
                firstLevel = false;
            }
            Debug.Log("Level hidden Loading completed");
            GameManager.Instance.eventManager.HiddenLoadingComplete.Invoke();

        }

        public void LoadedNotification()
        {
            currentLoaded = true;
        }

        public void ClearOld()
        {
            if (oldLevel != null)
            {
                oldLevel.SetActive(false);
                //Destroy(oldLevel);
            }
            oldLevel = CurrentLevelObj;
        }

        public void ClearLevel()
        {
            if (CurrentLevelObj != null)
            {
                DestroyImmediate(CurrentLevelObj);
            }
            for (int i = 0; i < levelPoint.childCount; i++)
            {
                GameObject destroyObject = levelPoint.GetChild(i).gameObject;
                DestroyImmediate(destroyObject);
            }

        }
    }


}
