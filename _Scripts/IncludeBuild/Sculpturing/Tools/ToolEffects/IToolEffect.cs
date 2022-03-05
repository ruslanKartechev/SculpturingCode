using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IToolEffect
{
    void PlayEffect();
    void SetEffectPosRot(Vector3 position, Quaternion rotation);
}
