using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3_Active : MonoBehaviour
{
    Skill3_Missile missileScript;

    ParticleSystem[] bombPS;

    Collider collider;

    private void Awake()
    {
        missileScript = GetComponentInChildren<Skill3_Missile>();

        bombPS = transform.GetChild(1).GetComponentsInChildren<ParticleSystem>(true);

        missileScript.isHit += () => {
            BombActive();
        };

        collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ActiveTime(3f));
        collider.enabled = false;
    }



    void BombActive()
    {
        for(int i =0; i < bombPS.Length; i++)
        {
            bombPS[i].Play();
        }
        StartCoroutine(EnableCollider());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            other.GetComponent<EnemyBase>().HP -= 50f;
        }
    }

    IEnumerator EnableCollider()
    {
        collider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        collider.enabled = false;
    }

    IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        SkillMissilePool.instance.ObjDisable(gameObject);
    }

}
