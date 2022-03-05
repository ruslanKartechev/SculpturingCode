using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiToolsEffect : MonoBehaviour, IToolEffect
{
    [SerializeField] private GameObject confettiPF;
    [SerializeField] private float scaleFactor;
    [SerializeField] private float duration;

    private GameObject effectInstance;
    private Vector3 pos = new Vector3();
    private Quaternion rot = new Quaternion();
    public void PlayEffect()
    {
        StartCoroutine(PlayingEffect());
    }
    public void SetEffectPosRot(Vector3 position, Quaternion rotation)
    {
        pos = position;
        rot = rotation;
    }

    private IEnumerator PlayingEffect()
    {
        if (effectInstance == null)
        {
            effectInstance = Instantiate(confettiPF);
        }
        else
        {
            effectInstance.SetActive(true);
        }
        effectInstance.transform.localScale = Vector3.one * scaleFactor;
        effectInstance.transform.position = pos;
        effectInstance.transform.rotation = rot;
        yield return new WaitForSeconds(duration);
        effectInstance.SetActive(false);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        if(effectInstance != null)
        {
            Destroy(effectInstance);
        }
    }
}
