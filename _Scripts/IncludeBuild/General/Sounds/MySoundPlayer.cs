using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using General;
public class MySoundPlayer : MonoBehaviour, ISoundEffect
{
    [SerializeField] private string mySoundName;

    public void PlayEffectOnce()
    {
        GameManager.Instance.soundManager.PlaySingleTime(mySoundName);
    }

    public void StartEffect()
    {
        GameManager.Instance.soundManager.StartSoundEffect(mySoundName);
    }

    public void StopEffect()
    {
        GameManager.Instance.soundManager.StopSoundEffect(mySoundName);
    }
}
