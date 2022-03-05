using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    public void Align()
    {
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }
}
