using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sculpturing;
using Sculpturing.Levels;
using Sculpturing.Tools;
using General.Data;
using General;
public class Tester : MonoBehaviour
{
    public ToolsManager tools;
    public GameObject toolPF;
    private void Start()
    {
        tools.InitTool(toolPF);
        GameManager.Instance.eventManager.CameraPositionSet.Invoke();
        GameManager.Instance.controlls.Init();
        GameManager.Instance.controlls.ResumeInput();
    }
}
