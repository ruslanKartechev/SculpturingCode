using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;

namespace Sculpturing.Tools
{

    [DefaultExecutionOrder(-1)]
    public class ToolMover : MonoBehaviour
    {
        [SerializeField] private LayerMask stage_1_mask;
        [SerializeField] private LayerMask stage_2_mask;
        [SerializeField] private string stage_1_center_name;
        private LayerMask pathMask;
        //[SerializeField] private string stage_2_center_name;
        [SerializeField] private string stage_3_center_name;
        [SerializeField] private string geometryCenterName;
        [Space(10)]
        [Header("Default Settings")]
        [SerializeField] private float DefaultMovementSensitivity;
        [SerializeField] private float rayRadius;
        [Header("Controlls settings")]
        [SerializeField] private float MovementSensitivity;
        [SerializeField] private float TesterSensitivity;
        [SerializeField] private bool UseOffset = true;
        [SerializeField] private bool UseAddtionalToolOffset = true;
        [Range(0f, 1f)]
        [SerializeField] private float MovementOffset;

        private Transform moveCenter;
        private Tool currentTool;
        private Transform toolTarget;
        private Coroutine movingRoutine;
        private Transform stage_1_center;
        private Transform stage_2_center;
        private Transform stage_3_center;
        private Transform statueGeometryCenter;

        private float currentOffset;
        
        [Space(10)]
        public GameObject TestPF;
        private Transform tester;
        private bool UseSphereCast = false;
        private bool DoMove;

        private void Start()
        {
            GameManager.Instance.eventManager.ToolInited.AddListener(OnToolInited);
            GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnLevelLoaded);
            GameManager.Instance.eventManager.LevelFinishInit.AddListener(TurnOff);
            GameManager.Instance.eventManager.InputStopped.AddListener(OnInputStopped);
            GameManager.Instance.eventManager.InputResumed.AddListener(OnInputResumed);
            if (tester == null)
                tester = Instantiate(TestPF).transform;
            CheckValues();
            
        }
        private void OnInputStopped()
        {
            TurnOff();
        }
        private void OnInputResumed()
        {
            InitMover();
        }
        private void CheckValues()
        {
            if (MovementSensitivity <= 0)
                Debug.Log("Movement Sensitivity is 0 or negative");
            if (DefaultMovementSensitivity <= 0)
                Debug.Log("Default Sensitivity is 0 or negative");
        }

        private void OnLevelLoaded()
        {
            if (stage_3_center != null)
                DestroyImmediate(stage_3_center.gameObject);

            stage_1_center = Helpers.FindByName(GameManager.Instance.data.currentLevelObjects.Stone.transform, stage_1_center_name);
            stage_1_center.gameObject.SetActive(false);
            stage_2_center = GameManager.Instance.data.currentLevelObjects.Statue.transform;

            stage_3_center = Helpers.FindByName(GameManager.Instance.data.currentLevelObjects.Statue.transform, stage_3_center_name);
            stage_3_center.gameObject.SetActive(false);
            statueGeometryCenter = Helpers.FindByName(GameManager.Instance.data.currentLevelObjects.SpawnTransform, geometryCenterName);
        }


        private void OnNewStage(StageData data)
        {
            TurnOff();
            UseSphereCast = false;
            switch (data.stageName)
            {
                case StageByName.Sculpturing:
                    pathMask = stage_1_mask;
                    moveCenter = stage_1_center;
                    moveCenter.gameObject.SetActive(true);
                    break;
                case StageByName.Polishing:
                    stage_1_center.gameObject.SetActive(false);
                    pathMask = stage_2_mask;
                    moveCenter = stage_2_center;
                    moveCenter.gameObject.SetActive(true);
                    // UseSphereCast = true;
                    UseSphereCast = false;
                    break;
                case StageByName.Painting:
                    pathMask = stage_1_mask; //  changed
                    moveCenter = stage_3_center; // changed
                    moveCenter.gameObject.SetActive(true);
                    UseSphereCast = false;
                    break;
                case StageByName.Decorating:
                    pathMask = stage_2_mask;
                    moveCenter = stage_2_center;
                    moveCenter.gameObject.SetActive(true);
                    UseSphereCast = false;
                    break;
            }

        }
        
        private void OnToolInited(Tool tool)
        {
            currentTool = tool;
            if (currentTool.UseToolMover == true)
            {
                InitMover();
                currentTool.SetMover(this);
            }
            else
                currentTool = null;
            currentOffset = 0;

        }
        public void InitMover()
        {
            if (currentTool == null) return;
            toolTarget = currentTool.gameObject.transform;
            movingRoutine = StartCoroutine(MovingAlongPath());
            GameManager.Instance.eventManager.MouseDown.AddListener(OnClick);
            GameManager.Instance.eventManager.MouseUp.AddListener(OnRelease);
        }


        public void TurnOff()
        {
            toolTarget = null;
            currentTool = null;
            DoMove = false;
            StopAllCoroutines();
            if (movingRoutine != null)
                StopCoroutine(movingRoutine);

            GameManager.Instance.eventManager.MouseDown.RemoveListener(OnClick);
            GameManager.Instance.eventManager.MouseUp.RemoveListener(OnRelease);
        }
        private void OnClick()
        {
            DoMove = true;
        }
        private void OnRelease()
        {
            SetTesterPosition(new Vector2(Screen.width/2, GetTesterPosition().y));
            DoMove = false;
        }
        private IEnumerator MovingAlongPath()
        {
            Vector3 currentPosition = Input.mousePosition;
            Vector3 lastPosition = Input.mousePosition;
            Vector3 delta = new Vector3();
            SetToolCenterPosition();
            while (true)
            {
                currentPosition = Input.mousePosition;
                lastPosition = currentPosition;
                while (DoMove)
                {
                    SetPrevPosition();
                    currentPosition = Input.mousePosition;
                    delta = currentPosition - lastPosition;
                    ScreenRaycastMove(delta);
                    MoveTester(delta);
                    lastPosition = currentPosition;
                    yield return null;
                }
                yield return null;
            }
        }

        // Tester movement
        public Vector2 GetTesterPosition()
        {
            return Camera.main.WorldToScreenPoint(tester.position);
        }

        private void MoveTester(Vector2 delta)
        {
            if (delta == Vector2.zero)
                return;
            Vector2 currentPosition = Camera.main.WorldToScreenPoint(tester.position);
            Vector2 newPos = currentPosition + delta* TesterSensitivity;
            #region Screenbounds
            if (newPos.x <= 0)
            {
                if (newPos.x <= currentPosition.x)
                    return;
            } else if (newPos.x >= Screen.width)
            {
                if (newPos.x >= currentPosition.x)
                    return;
            }
            if (newPos.y <= 0)
            {
                if (newPos.y <= currentPosition.y)
                    return;
            }
            else if (newPos.y >= Screen.height)
            {
                if (newPos.y >= currentPosition.y)
                    return;
            }
            #endregion
            SetTesterPosition(newPos);
        }

        private void SetTesterPosition(Vector2 screenPos)
        {
            Vector3 worldPos = new Vector3(screenPos.x, screenPos.y,1);
            tester.position = Camera.main.ScreenToWorldPoint(worldPos);
        }
        public void SetTesterCentered()
        {
            SetTesterPosition(new Vector2(Screen.width/2,Screen.height/2));
        }

        private Vector2 GetGeometryCenterScreenPos()
        {
            return Camera.main.WorldToScreenPoint(statueGeometryCenter.position);
        }

        // Tool movement

        public Vector2 GetToolPosition()
        {
            return Camera.main.WorldToScreenPoint(toolTarget.position+toolTarget.forward* currentOffset);
        }

        public void SetToolCenterPosition()
        {
            SetTesterPosition(new Vector2(Screen.width/2, Screen.height/2));
            Ray ray = Camera.main.ScreenPointToRay(GetGeometryCenterScreenPos());
            CastAndMove(ray,true);
        }
        private void SetPrevPosition()
        {
            Ray ray2 = Camera.main.ScreenPointToRay(GetToolPosition());
            CastAndMove(ray2, false);
        }
        public void SetToolToMousePosition()
        {
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            CastAndMove(ray2, false);
        }
        private void ScreenRaycastMove(Vector2 delta)
        {
            if (delta == Vector2.zero)
                return;
            
            delta *= MovementSensitivity;
            if (currentTool.SensitivityModificator > 0)
                delta *= currentTool.SensitivityModificator;
            Vector2 newScreenPos = GetToolPosition() + delta;
            #region ScreenBounds
            if (newScreenPos.x <= 0)
            {
                newScreenPos.x = 0;
            } else if (newScreenPos.x >= Screen.width)
            {
                newScreenPos.x = Screen.width;
            }
            if(newScreenPos.y <= 0)
            {
                newScreenPos.y = 0;
            }else if(newScreenPos.y >= Screen.height)
            {
                newScreenPos.y = Screen.height;
            }
            #endregion
            Ray ray = Camera.main.ScreenPointToRay(newScreenPos);
            if(CastAndMove(ray, UseSphereCast) == false)
            {
                CastAndMove(ray, true);
            }
        }
        private bool CastAndMove(Ray ray, bool sphereCast = false)
        {
            if (currentTool == null)
                return false;
            RaycastHit hit;
            bool didHit = false;
            if (UseOffset)
                currentOffset = MovementOffset;
            if (UseAddtionalToolOffset)
                currentOffset += currentTool.AdditionalOffset;

            if (sphereCast == true)
            {
                if (Physics.SphereCast(ray, rayRadius, out hit, 20, pathMask))
                    didHit = true;
                else
                    return false;
            }
            else
            {
                if (Physics.Raycast(ray, out hit, 20, pathMask))
                    didHit = true;
                else
                    didHit = false;
            }
            if(didHit == true)
            {
                toolTarget.position = hit.point;
                Vector3 LookVector = -hit.normal;                   
                toolTarget.rotation = Quaternion.LookRotation(LookVector);
                if (UseOffset)
                    toolTarget.position += hit.normal * currentOffset;
            }
            return didHit;
        }
    }
}