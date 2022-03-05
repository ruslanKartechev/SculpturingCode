using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Sculpturing.Tools {
    [DefaultExecutionOrder(4)]
    public class StatueRotator : MonoBehaviour
    {
        [SerializeField] private float RegionRadius = 300;
        [Tooltip("Measured as a percent of screen width")]
        [SerializeField] private float VerticalRotationDeadzone = 0.1f;
        [SerializeField] private float speed_min;
        [SerializeField] private float speed_max;
        [Space(10)]
        [SerializeField] private bool UseVerticalTilt = true;
        [SerializeField] private float maxTintAmount = 10f;
        private float backRotationTime;

        private Transform spawn;
        private Transform rotatable;
        private bool IsActived;

        private Vector2 testerPosition;
        private Vector2 screenCenter;

        private Quaternion StatueStartRot = new Quaternion();
        private Quaternion SpawnStartRot = new Quaternion();
        [SerializeField] private ToolMover toolMover;
        private Coroutine rotationRoutine;
        private void Start()
        {
            if(toolMover == null)
                toolMover = GetComponent<ToolMover>();
            GameManager.Instance.eventManager.StageFinished.AddListener(OnStageFinished);
            GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
            GameManager.Instance.eventManager.LevelLoaded.AddListener(OnNewLevel);

            screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
            IsActived = false;
            backRotationTime = GameManager.Instance.data.FinishingSequence.StatueBackRotationTime;
        }
        private void OnNewLevel()
        {
            DisableRotation();
            spawn = GameManager.Instance.data.currentLevelObjects.SpawnTransform;
            rotatable = spawn.transform.GetChild(0);
            StatueStartRot = rotatable.localRotation;
            SpawnStartRot = spawn.localRotation;
        }
        private void OnNewStage(StageData data)
        {
            GameManager.Instance.eventManager.CameraMovementStarted.AddListener(OnCameraMovementStart);
        }
        // Pre rotate to face Camera
        private void OnCameraMovementStart(RotationData data)
        {
            GameManager.Instance.eventManager.CameraMovementStarted.RemoveListener(OnCameraMovementStart);
            if (rotatable != null)
                StartCoroutine(RotateToFace(data.rotationTime, data.target));
        }
        private void OnStageFinished()
        {
            DisableRotation();
        }
        public void OnLevelFinish()
        {
            StartCoroutine(RotateBack(backRotationTime));
        }
        private void EnableRotation()
        {
            if (rotationRoutine != null)
                StopCoroutine(rotationRoutine);
            rotationRoutine = StartCoroutine(RotationRoutine());
            IsActived = true;
        }
        private void DisableRotation()
        {
            IsActived = false;
            if (rotationRoutine != null)
                StopCoroutine(rotationRoutine);
        }
        private IEnumerator RotateBack(float time)
        {
            if (time < 1.2f)
                time = 1.2f;
            DisableRotation();
            float timeElapsed = 0;
            Quaternion spawnStart = spawn.localRotation;
            Quaternion spawnEnd = SpawnStartRot;
            Quaternion endRot = StatueStartRot;
            Quaternion startRot = rotatable.localRotation;
            while (timeElapsed <= time)
            {
                spawn.localRotation = Quaternion.Lerp(spawnStart, spawnEnd, 2 * timeElapsed / time);
                rotatable.localRotation = Quaternion.Lerp(startRot, endRot, 2 * timeElapsed / time);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            spawn.localRotation = SpawnStartRot;
            rotatable.localRotation = StatueStartRot;

            //float timeElapsed = 0;
            //Quaternion spawnStart = spawn.localRotation;
            //Quaternion spawnEnd = SpawnStartRot;
            //while (timeElapsed < time / 2)
            //{
            //    spawn.localRotation = Quaternion.Lerp(spawnStart, spawnEnd, 2 * timeElapsed / time);
            //    timeElapsed += Time.deltaTime;
            //    yield return null;
            //}
            //spawn.localRotation = SpawnStartRot;
            //Quaternion endRot = StatueStartRot;
            //Quaternion startRot = rotatable.localRotation;
            //while (timeElapsed < time/2)
            //{
            //    rotatable.localRotation = Quaternion.Lerp(startRot, endRot, 2*timeElapsed / time);
            //    timeElapsed += Time.deltaTime;
            //    yield return null;
            //}
            //rotatable.localRotation = endRot;
        }

        private IEnumerator RotateToFace(float time, Transform target)
        {
            if (time <= 0.6f) { time = 0.6f; }
            float timeElapsed = 0;
            Quaternion spawnStart = spawn.rotation;
            Vector3 lookVector = new Vector3(target.position.x, spawn.position.y, target.position.z) - spawn.position;
            Quaternion spawnEnd = Quaternion.LookRotation(lookVector);
            while (timeElapsed < time/2)
            {
                spawn.rotation = Quaternion.Lerp(spawnStart, spawnEnd, 2*timeElapsed / time);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            //spawn.rotation = spawnEnd;
            timeElapsed = 0;
            lookVector = new Vector3(target.position.x, rotatable.position.y, target.position.z) - rotatable.position;
            Quaternion endRot = Quaternion.LookRotation(lookVector.normalized, spawn.up);
            Quaternion startRot = rotatable.rotation;

            while(timeElapsed < time/2)
            {
                rotatable.rotation = Quaternion.Lerp(startRot, endRot, 2*timeElapsed/ time);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
          //  yield return null;
            EnableRotation();
        }

        private IEnumerator RotationRoutine()
        {
            toolMover.SetTesterCentered();
            rotatable.localEulerAngles = new Vector3(rotatable.localEulerAngles.x, 0,0);
            float startingAngle = spawn.localEulerAngles.x;
            float angle = 0f;
            while (true)
            {
                if (IsActived)
                {
                    testerPosition = toolMover.GetTesterPosition();
                    if (CheckPosition(testerPosition) == false && (Mathf.Abs(RelativePosition(testerPosition).x) > Screen.width * VerticalRotationDeadzone))
                    {
                        float rotAmount = CalculateVelocity(testerPosition);
                        if (RelativePosition(testerPosition).x < 0)
                            rotAmount *= -1;
                        rotatable.localEulerAngles += new Vector3(0, rotAmount, 0);

                    }
                    if (UseVerticalTilt == true ) 
                    {
                        if (RelativePosition(testerPosition).y > 0)
                        {
                            angle = Mathf.Lerp(0, maxTintAmount, RelativePosition(testerPosition).y / (Screen.width / 2));
                        }
                        else if (RelativePosition(testerPosition).y < 0)
                        {
                            angle = -Mathf.Lerp(0, maxTintAmount, -RelativePosition(testerPosition).y / (Screen.width / 2));
                        }
                      //  Debug.Log(angle);
                        spawn.localEulerAngles = new Vector3(startingAngle+angle, spawn.localEulerAngles.y, spawn.localEulerAngles.z);
                    }
                }
                yield return null;
            }
        }

        private float CalculateVelocity(Vector2 toolPos)
        {
            float rad = CurrentRadius(toolPos);
            return Mathf.Lerp(speed_min, speed_max, rad / Screen.width / 2) * Time.deltaTime;
        }

        private bool CheckPosition(Vector2 position)
        {
            bool IsWhithin = true;
            if (CurrentRadius(position) >= RegionRadius)
                IsWhithin = false;

            return IsWhithin;
        }
        private float CurrentRadius(Vector2 position)
        {
            float rad = RelativePosition(position).magnitude;
            return rad;
        }
        private Vector2 RelativePosition(Vector2 position)
        {
            return position - screenCenter;
        }

    }
}
