#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CameraHelper))]
public class CameraHelperGUI : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CameraHelper me = (CameraHelper)target;
        if (GUILayout.Button("Align"))
        {
            me.Align();
        }
    }
}
#endif