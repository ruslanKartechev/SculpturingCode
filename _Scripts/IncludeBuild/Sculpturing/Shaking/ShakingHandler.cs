using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingHandler : MonoBehaviour
{
    [Range(0f, 0.2f)]
    [SerializeField] protected float BaseMultiplier;
    [SerializeField] protected bool UseShakingModifyer;
    [SerializeField] protected float ShakingDuration;
    [SerializeField] protected AnimationCurve strengthCurve;
    protected Transform target;
    protected bool IsShaking;

    public void OnImpact(int magnitude)
    {
        if(target == null)
        { Debug.Log("target object not set");return; }
        StartShake(magnitude);
      //  Debug.Log("IMpact recieved");
    }
    public void StartShake(int modifyer = 0)
    {
        if (IsShaking == true)
            return;
        float mod = modifyer;
        if (mod <= 5)
            mod *= -1;
        mod *= 0.1f;
        StartCoroutine(Shaking(mod));

    }

    private IEnumerator Shaking(float modifyer)
    {
        IsShaking = true;
        float elapsedTime = 0f;
        Vector3 startPosition = target.position;
        while (elapsedTime < ShakingDuration)
        {

            elapsedTime += Time.deltaTime;
            float strength = strengthCurve.Evaluate(elapsedTime / ShakingDuration);
            target.transform.position = startPosition + Random.onUnitSphere * (strength + modifyer) * BaseMultiplier;
            yield return null;
        }
        target.position = startPosition;
        IsShaking = false;

    }
}
