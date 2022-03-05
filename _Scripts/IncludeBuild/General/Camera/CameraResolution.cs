using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sculpturing.Cam
{
    [ExecuteInEditMode]
    public class CameraResolution : MonoBehaviour
    {
        private Camera mCam;
        [SerializeField] private int MinFov;
        [SerializeField] private int MaxFov;
        void Awake()
        {
            mCam = Camera.main;
            InitCameraPerspective();
        }
        private void InitCameraPerspective()
        {
            float ratio = (float)Screen.width / Screen.height;
            Debug.Log("<color=red>Screen: </color>" + new Vector2(Screen.width, Screen.height));
            Debug.Log("<color=red>Ratio: </color>" + ratio);
            mCam.fieldOfView = (int)Mathf.Lerp(MaxFov, MinFov, ratio / 0.85f);
        }
    }
}