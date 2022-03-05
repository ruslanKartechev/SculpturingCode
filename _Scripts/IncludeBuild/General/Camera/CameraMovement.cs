using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;


public class RotationData
{
    public Transform target;
    public float rotationTime;
    public RotationData(Transform pos, float time)
    {
        target = pos;
        rotationTime = time;
    }
}


namespace Sculpturing.Cam
{
    [DefaultExecutionOrder(5)]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 startingPosition = new Vector3();
        [SerializeField] private Vector3 defaultPosition = new Vector3();
        private Vector3 defaultRotation = Quaternion.LookRotation( -Vector3.forward, Vector3.up).eulerAngles;
        private Transform Camera;
        [SerializeField] private float moveSpeed;
        [SerializeField] private bool pointsDirectionFlip = true;
        private CameraPositions positions = new CameraPositions();
        private CameraManager manager;
        private Transform currentTarget;
        private bool startPositionSet = false;
        public void Init(CameraManager _manager, Transform cam)
        {
            manager = _manager;
            if (Camera == null && cam == null)
                Camera = UnityEngine.Camera.main.transform;
            else
                Camera = cam;
            Camera.transform.position = startingPosition;
        }

        public void SetCameraPositionDefault()
        {
            Camera.transform.position = defaultPosition;
        }
        public void SetCameraRotationDefault()
        {
            //Camera.transform.rotation = Quaternion.LookRotation(Vector3.up, -Vector3.forward);
            Camera.transform.eulerAngles = defaultRotation;
        }


        public void OnLevelLoaded()
        {
            Transform level = GameManager.Instance.data.currentLevel.transform;
            startPositionSet = false;
            positions = new CameraPositions();
            positions.pos_0 = FindObject(level, DataManager.CameraPosName + "0");
            positions.pos_1 = FindObject(level, DataManager.CameraPosName + "1");
            positions.pos_2 = FindObject(level, DataManager.CameraPosName + "2");
            positions.pos_3 = FindObject(level, DataManager.CameraPosName + "3");
            positions.pos_4 = FindObject(level, DataManager.CameraPosName + "4");
            if (positions.pos_0 == null)
            {
                Debug.Log("Camera pos_0 not found.Default parameters are used");
                SetCameraPositionDefault();
                SetCameraRotationDefault();
                startPositionSet = true;
                GameManager.Instance.eventManager.CameraStartPositionSet.Invoke();
            }
            else
            {
                AlignImmiadiate(Camera, positions.pos_0);
            }
        }
        public void OnStageChange(StageData data)
        {
            if (data.stageName == StageByName.Sculpturing)
            {
                currentTarget = positions.pos_1;
                return;
            }
            else if (data.stageName == StageByName.Polishing)
            {
                currentTarget = positions.pos_2;
            }
            else if (data.stageName == StageByName.Painting)
            {
                currentTarget = positions.pos_3;
            } else if (data.stageName == StageByName.Decorating)
            {
                currentTarget = positions.pos_3;
            }
            if(currentTarget != null)
            {
                StopAllCoroutines();
                StartCoroutine(AlignToPoint(Camera, currentTarget, moveSpeed));
            }
        }


        public void OnGamePlay()
        {
            if(currentTarget == null)
            {
                Debug.Log("first Stage Positiong not set");
                Camera.transform.position = defaultPosition;
                Camera.transform.eulerAngles = defaultRotation;
                return;
            }
            Quaternion targetRot = new Quaternion();
            if (pointsDirectionFlip == true)
                targetRot.SetLookRotation(-currentTarget.forward, currentTarget.up);
            else
                targetRot.SetLookRotation(currentTarget.forward, currentTarget.up);

            StartCoroutine(AlignToPoint(Camera,currentTarget,moveSpeed));
        }


        public void GoToLevelEndPos()
        {
            currentTarget = positions.pos_4;
            float time = GameManager.Instance.data.FinishingSequence.CameraBackMovementTime;
            if (time < 1f)
                time = 1f;
            float speed = (transform.position - currentTarget.position).magnitude / time;
            StartCoroutine(AlignToPoint(Camera, currentTarget, speed));
        }

        private Transform FindObject(Transform parent, string name)
        {
            Transform target = null;
            foreach (Transform child in parent)
            {
                if (child.name.Contains(name))
                    target = child;
            }

            return target;
        }


        private void AlignImmiadiate(Transform moveObject, Transform target)
        {
            moveObject.position = target.position;
            moveObject.rotation = target.rotation;
            if (startPositionSet == false)
            {
                startPositionSet = true;
                GameManager.Instance.eventManager.CameraStartPositionSet.Invoke();
            }
            else
            {
                GameManager.Instance.eventManager.CameraPositionSet.Invoke();
            }
        }

        private IEnumerator AlignToPoint(Transform moveObject, Transform target, float speed)
        {
            Quaternion startRot = moveObject.rotation;
            Quaternion endRot = target.rotation;
            Vector3 startPos = moveObject.position;
            Vector3 endPos = target.position;
            float distance = (endPos - startPos).magnitude;
            float timeToMove = distance / speed;
            float timeElapsed = 0f;
            yield return null;
            GameManager.Instance.eventManager.CameraMovementStarted.Invoke(new RotationData(target,timeToMove));
            while (timeElapsed < timeToMove)
            {
                moveObject.position = Vector3.Lerp(startPos, endPos, timeElapsed / timeToMove);
                moveObject.rotation = Quaternion.Slerp(startRot, endRot, timeElapsed/ timeToMove);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            moveObject.position = endPos;
            moveObject.rotation = target.rotation;
            yield return null;
            if (startPositionSet == false)
            {
                startPositionSet = true;
                GameManager.Instance.eventManager.CameraStartPositionSet.Invoke();
            }
            else
            {
                GameManager.Instance.eventManager.CameraPositionSet.Invoke();
            }
            
        }




    }



    [System.Serializable]
    public class CameraPositions
    {
        public CameraPositions()
        {
            pos_1 = pos_2 = pos_3 = null;
        }
        public Transform pos_0;
        public Transform pos_1;
        public Transform pos_2;
        public Transform pos_3;
        public Transform pos_4;
    }
}