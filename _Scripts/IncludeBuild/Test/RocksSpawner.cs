using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksSpawner : MonoBehaviour
{
    [Tooltip("Small rocks prefab")]
    [SerializeField] private List<GameObject> RockVarians = new List<GameObject>();
    [Tooltip("Leave only the 'Polish' layer")]
    [SerializeField] private LayerMask layerMask;
    [Tooltip("set based on original size of the piece prefab")]
    [Header("Rocks Scale")]
    [Range(0f,1f)]
    [SerializeField] private float RocksBaseScale = 0.75f;

    [SerializeField] private bool variableScale = true;
    [Range(0f, 1f)]
    [SerializeField] private float scale_min = 0.5f;
    [Range(1f,2f)]
    [SerializeField] private float scale_max = 1.5f;
    private GameObject target;
    private List<RaycastHit> hitPoints = new List<RaycastHit>();
    public List<GameObject> spawnedRocks { get; private set; }

    public void Init(GameObject _target)
    {
        spawnedRocks = new List<GameObject>();
        target = _target;
        Ray ray = new Ray();
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 camForward = Camera.main.transform.forward;
        ray.direction = Camera.main.transform.forward;
        ray.origin = cameraPos;
        for(int i=0; i<300; i++)
        {
            Vector3 dir = camForward;
            dir.x = camForward.x + Random.Range(-0.1f, 0.1f);
            dir.y = camForward.y + Random.Range(-0.1f, 0.3f);
            ray.direction = dir;
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 15, ~layerMask))
            {
                hitPoints.Add(hit);
            }
        }
    }
    public void SpawnRocks(bool show = true)
    {
        foreach(RaycastHit hit in hitPoints)
        {
            GameObject rock = Instantiate(ChooseRandomObject(RockVarians));
            rock.layer = 10;
            rock.transform.localScale *= RocksBaseScale;
            rock.AddComponent<SphereCollider>();
            if (variableScale)
            {
                float scale = Random.Range(scale_min, scale_max);
                rock.transform.localScale *= scale;
            }
            rock.transform.parent = target.transform;
            rock.transform.position = hit.point;
            rock.transform.eulerAngles = new Vector3(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));

            if (show == false)
                rock.SetActive(false);
            spawnedRocks.Add(rock);
        }
    }
    
    private GameObject ChooseRandomObject(List<GameObject> samples)
    {
        int rand = Random.Range(0, samples.Count);
        return samples[rand];
    }
}
