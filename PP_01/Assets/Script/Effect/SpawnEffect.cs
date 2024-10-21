using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(Effect());
    }

    private void OnDisable()
    {
        //StopAllCoroutines();
        ps.Stop();
    }

    IEnumerator Effect()
    {
        ps.Play();
        yield return new WaitForSeconds(1f);

        ps.Stop();
        SpawnEffectPool.instance.ObjDisable(gameObject);
    }
}
