using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using UnityEditor;
namespace Sculpturing
{
    public class VertexRockSpawner : MonoBehaviour
    {

       [HideInInspector] public Transform CurrentShardRoot;
       [HideInInspector] public List<Transform> Shards;

        [SerializeField] private List<GameObject> RockVariants = new List<GameObject>();
        private bool variableScale = true;
        private float fixedScale = 0.5f;
        private float scale_min = 0.5f;
        private float scale_max = 1.5f;
        private int spacing = 15;
        private int layer;
        private MeshFilter filter;
        private Mesh mesh;
        private Vector3[] vertices;
        private GameObject target;
        public void Init(GameObject _target, RockShardsSpawningData data)
        {
            variableScale = data.variableScale;
            fixedScale = data.fixedScale;
            scale_max = data.scale_max;
            scale_min = data.scale_min;
            RockVariants = data.RockVariants;
            spacing = data.spacing ;
            target = _target;
            layer = data.rocksLayer;
            filter = target.GetComponent<MeshFilter>();
            mesh = filter.sharedMesh;
            vertices = mesh.vertices;
        }
        public PolishingData SpawnRocks()
        {

            Shards = new List<Transform>(); 
            int count = 0;
            CurrentShardRoot = new GameObject("RocksRoot").transform;
            CurrentShardRoot.transform.parent = target.transform;
            CurrentShardRoot.localPosition = Vector3.zero;
            CurrentShardRoot.localEulerAngles = Vector3.zero;
            CurrentShardRoot.localScale = Vector3.one;
            PolishingData spawnedData = new PolishingData();

            spawnedData.shards = new List<RockShardsData>();

            string rocksPath = "Assets/Resources/GeneratedStones/Stage_2/" + target.name;
            string innerPath = "GeneratedStones/Stage_2/" + target.name;
#if UNITY_EDITOR
            if (AssetDatabase.IsValidFolder(rocksPath))
            {
                AssetDatabase.DeleteAsset(rocksPath);
            }
            AssetDatabase.CreateFolder("Assets/Resources/GeneratedStones/Stage_2", target.name);
            int k = 0;
            List<string> instNames = new List<string>();
            for (int i = 0; i < vertices.Length; i += spacing)
            {
                RockShardsData shardData = new RockShardsData();
                count++;
                Vector3 pos = vertices[i];
                GameObject rock = Instantiate(ChooseRandomObject(RockVariants));
                string name = rock.name.Replace("(Clone)", "")  + "_ShardPF";
                shardData.path = innerPath + "/" + name;
                if (instNames.Contains(rock.name) == false)
                {
                    instNames.Add(rock.name);
                    PrefabUtility.SaveAsPrefabAsset(rock, rocksPath + "/" + name + ".prefab");
                }

                if (variableScale)
                    rock.transform.localScale *= Random.Range(scale_min, scale_max);
                else
                    rock.transform.localScale *= fixedScale;
                rock.transform.parent = target.transform;
                rock.transform.localPosition = pos;
                rock.transform.eulerAngles = RandomRotation();
             //   rock.layer = layer;
             //   rock.AddComponent<SphereCollider>();
                rock.transform.parent = CurrentShardRoot;
                shardData.localPos = rock.transform.localPosition;
                shardData.localEulers = rock.transform.localEulerAngles;
                shardData.localScale = rock.transform.localScale.x;
                spawnedData.shards.Add(new RockShardsData(shardData));
                k++;

                Shards.Add(rock.transform);
            }
            ClearSpawned();
#endif

            return spawnedData;
        }
        public void AnalyzeVertices(GameObject statue)
        {
            Vector3[] verts = statue.GetComponent<MeshFilter>().sharedMesh.vertices;
            int count = verts.Length;
            Debug.Log("<color=blue>The number of vertices: " + count + "/color");
        }
        public void ClearSpawned()
        {
            target = null;
            Shards = null;
            filter = null;
            mesh = null;
            if(CurrentShardRoot != null)
            {
                DestroyImmediate(CurrentShardRoot.gameObject);
                CurrentShardRoot = null;
            }

        }
        public Vector3 RandomRotation()
        {
            return new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
        private GameObject ChooseRandomObject(List<GameObject> samples)
        {
            int rand = Random.Range(0, samples.Count);
            return samples[rand];
        }
    }

}
