


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using UnityEditor;
using RayFire;
namespace Sculpturing
{
  
    public class StoneSpawner : MonoBehaviour
    {
        [HideInInspector] public SculpturingData CurrentData;
        public int StoneLayer = 8;
        // piece root is set automatically inside StonePiece
        private GameObject spawnedRoot;

        public SculpturingData SpawnStone(GameObject rawStone, List<StonesShatterData> data)
        {
            if (rawStone == null) { Debug.Log("stone movel not passed"); return null; }
            ClearMainStone(rawStone);
            rawStone.layer = StoneLayer;
            CurrentData = new SculpturingData();
            CurrentData.mainStone = rawStone.AddComponent<StonePiece>();
            CurrentData.mainStone.SetData(0, null);
            CurrentData.mainStone.InitAsMainStone();

            StonePieceLoader mainStoneLoader = rawStone.AddComponent<StonePieceLoader>();

            List<GameObject> t1 = PreCrack(data[0], rawStone) ; // list of tier_1 pieces
            spawnedRoot = t1[0].transform.parent.gameObject;
            string resources = "Assets/Resources/";
            string folderPath_1 = "GeneratedStones/Tier_1/" + rawStone.name;
            string folderPath_2 = "GeneratedStones/Tier_2/" + rawStone.name;
#if UNITY_EDITOR
            if (AssetDatabase.IsValidFolder(resources+folderPath_1))
            {
                AssetDatabase.DeleteAsset(resources+folderPath_1);
            }
            if (AssetDatabase.IsValidFolder(resources+folderPath_2))
            {
                AssetDatabase.DeleteAsset(resources+folderPath_2);
            }
            string StonePath = "StonePieces/";
            string MeshPath = "Meshes/";
            AssetDatabase.CreateFolder(resources + "GeneratedStones/Tier_1", rawStone.name);
            AssetDatabase.CreateFolder(resources + "GeneratedStones/Tier_2", rawStone.name);
            AssetDatabase.CreateFolder(resources + folderPath_1, "StonePieces");
            AssetDatabase.CreateFolder(resources + folderPath_1, "Meshes");
            AssetDatabase.CreateFolder(resources + folderPath_2, "StonePieces");
            AssetDatabase.CreateFolder(resources + folderPath_2, "Meshes");
        //    Debug.Log("<color=blue>Created folder 1 at: " + resources + folderPath_1 + "</color>");
           // Debug.Log("<color=blue>Created folder 2 at: " + resources + folderPath_2 + "</color>");
            
            foreach (GameObject stone in t1)
            {
                // tier 1
                string name = stone.name;
                string path1 = folderPath_1 + "/" + StonePath + name; // path inside resources
                string meshP = folderPath_1 + "/" + MeshPath + name;
                mainStoneLoader.AddChildPath(path1);

                StonePiece piece1 = stone.AddComponent<StonePiece>();
                AssetDatabase.CreateAsset(stone.GetComponent<MeshFilter>().sharedMesh, resources + meshP + ".asset");
                AssetDatabase.SaveAssets();
                piece1.SetData(1,meshP);
                StonePieceLoader loader_1 = stone.AddComponent<StonePieceLoader>();
                // tier 2
                List<GameObject> t2 = PreCrack(data[1], stone); // list of tier_2 pieces
                foreach (GameObject stone2 in t2)
                {
                    name = stone2.name;
                    string path2 = folderPath_2 +"/" + StonePath +  name;
                    meshP = folderPath_2 + "/"+ MeshPath +  name;
                    loader_1.AddChildPath(path2);
                    StonePiece piece2 = stone2.AddComponent<StonePiece>();
                    AssetDatabase.CreateAsset(stone2.GetComponent<MeshFilter>().sharedMesh, resources + meshP + ".asset");
                    AssetDatabase.SaveAssets();
                    piece2.SetData(2, meshP);
                    StonePieceLoader loader_2 = stone2.AddComponent<StonePieceLoader>();
                    piece2.InitTier2();
                    loader_2.InitEmpty();
                    PrefabUtility.SaveAsPrefabAsset(stone2, resources + path2 + ".prefab");
                }
                piece1.InitTier1();
                PrefabUtility.SaveAsPrefabAsset(stone, resources + path1 + ".prefab");
            }
#endif
            return CurrentData;
        }

        public void ClearMainStone(GameObject mainStone)
        {
            StonePiece piece = mainStone.GetComponent<StonePiece>();
            if (piece != null)
                DestroyImmediate(piece);
            StonePieceLoader loader = mainStone.GetComponent<StonePieceLoader>();
            if (loader != null)
                DestroyImmediate(loader);
            
        }
        
        
        public void Clear()
        {
            DestroyImmediate(spawnedRoot);
            spawnedRoot = null;
        }

        public List<GameObject> PreCrack(StonesShatterData data, GameObject target)
        {
            RayfireShatter shatter = target.gameObject.AddComponent(typeof(RayfireShatter)).GetComponent<RayfireShatter>();
            shatter.mode = FragmentMode.Editor;
            shatter.voronoi.amount = data.ShatterAmount;
            shatter.advanced.decompose = false;
            shatter.type = FragType.Voronoi;
            shatter.advanced.elementSizeThreshold = data.MinSize;
            shatter.advanced.seed = Random.Range(1, 30);
            List<GameObject> shatteredPieces = new List<GameObject>();
            shatter.fragmentsAll = shatteredPieces;
            shatter.Fragment();
            if (shatteredPieces.Count == 0 && data.Tier != 2)
            {
                Debug.Log("Problem with shattering: " + gameObject.name);
                return null;
            }
            for (int i = 0; i < shatteredPieces.Count; i++)
            {
                shatteredPieces[i].transform.localScale *= data.LocalScale;
            }
            DestroyImmediate(shatter);
            return shatteredPieces;
        }






    }
}