using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshCheck : MonoBehaviour
{

    public MeshFilter model_1;
    public MeshFilter model_2;

    private void Start()
    {
        Mesh mesh1 = model_1.mesh;
        Mesh mesh2 = model_2.mesh;
        Debug.Log("Working model");
        Debug.Log("normals: " + mesh1.vertices.Length);
        Debug.Log("vertices count: " + mesh1.vertices.Length);
        Debug.Log("mesh uvs " + mesh1.uv.Length);
        Debug.Log("Not working model:");
        Debug.Log("normals: " + mesh2.vertices.Length);
        Debug.Log("vertices count: " + mesh2.vertices.Length);
        Debug.Log("mesh uvs " + mesh2.uv.Length);

    }

}
