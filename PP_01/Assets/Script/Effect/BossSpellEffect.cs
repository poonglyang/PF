using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpellEffect : MonoBehaviour
{
    ParticleSystem[] BossSpellPS;

    Transform playerManager;

    Rigidbody rigid;

    public float shootSpeed = 1f;
    private void Awake()
    {
        BossSpellPS = GetComponentsInChildren<ParticleSystem>();
        playerManager = FindAnyObjectByType<PlayerManager>().transform;

        rigid = GetComponent<Rigidbody>();

        //Time.timeScale = 0.3f;
    }

    private void OnEnable()
    {

        for(int i= 0; i < BossSpellPS.Length; i ++)
        {
            BossSpellPS[i].Play();
        }
        rigid.AddForce(shootSpeed * (playerManager.position - transform.position + 2 * Vector3.up), ForceMode.VelocityChange);

        StartCoroutine(ActiveTime(3f));
    }

    private void OnDisable()
    {
        for (int i = 0; i < BossSpellPS.Length; i++)
        {
            BossSpellPS[i].Stop();
        }
    }

    protected IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        PistolBulletPool.instance.ObjDisable(gameObject);
    }


}
