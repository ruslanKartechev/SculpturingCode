
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PreGenTool))]
public class PreGenToolGUI : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PreGenTool tool = (PreGenTool)target;


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate all"))
        {
            tool.PreGen();
        }
        if (GUILayout.Button("Analyze"))
        {
            tool.DebugAnalyze();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Stage 1"))
        {
            tool.SoloGenStage1(true);
        }
        if (GUILayout.Button("Generate Stage 2"))
        {
            tool.SoloGenStage2(true);
        }
        GUILayout.EndHorizontal();


    }
}
# endif