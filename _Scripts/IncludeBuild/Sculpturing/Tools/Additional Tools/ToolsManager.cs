using System.Collections.Generic;
using UnityEngine;
using General.Data;
using General;
namespace Sculpturing.Tools
{
    public class ToolsManager : MonoBehaviour
    {

        [Header("References")]
        public ToolMover toolMover;
        public StatueRotator statueRotator;
        public PaintableManager paintableManager;
        public StatueOutliner outliner;
        public MaterialsManager materialsManager;


        [Header("StageFinishButton")]
        [Tooltip("Instantiated every level")]
        [SerializeField] private GameObject stageFinishBtn;
        private GameObject finishBtnInst;
        [Space(10)]
        private ITool currentTool = null;
        private GameObject toolObj = null;

        private List<GameObject> toolInst = new List<GameObject>();
        private List<ITool> toolsInited = new List<ITool>();
        public GameObject CurrentToolObject { get { return toolObj; } } 

        public void Init()
        {
            GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnNewlevel);
            Vibration.Init();
            finishBtnInst = Instantiate(stageFinishBtn);
        }
        private void OnNewlevel()
        {
            if(finishBtnInst == null)
            {
                finishBtnInst = Instantiate(stageFinishBtn);
            }
        }
        private void OnNewStage(StageData data)
        {
            InitTool(data.stageTool);
        }

        public void InitTool(GameObject _toolObj)
        {
            if(currentTool != null)
            {   
                currentTool.Disable();
            }
            toolObj = null;
            toolObj = toolInst.Find(x => x.name.Contains(_toolObj.name));
            if(toolObj == null)
            {
                toolObj = Instantiate(_toolObj);
                toolInst.Add(toolObj);
                currentTool = toolObj.GetComponent<ITool>();
                toolsInited.Add(currentTool);
            }
            currentTool = toolObj.GetComponent<ITool>();
            if (currentTool == null) { Debug.Log("tool script not assigned"); return; }
            toolObj.SetActive(true);
            currentTool.Init();
        }

        public void Clear()
        {
            currentTool.Disable();
            currentTool = null;
            toolObj = null;
        }


    }
}