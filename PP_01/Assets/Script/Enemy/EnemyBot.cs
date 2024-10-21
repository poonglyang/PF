using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemyBot : EnemyBase
{
    Skill skillPanel;

    protected override void AwakePlus()
    {
        skillPanel = FindAnyObjectByType<Skill>();
    }

    protected override void WhenHit()
    {
        if(HP < 1)
        {
            skillPanel.ResorceIncrease(0.01f);
            GameOverPanel.instance.commonZombDieCount++;
            if (Random.value < goodsDrop)
            {
                CoinPool.instance.SetActiveObject(transform.position + Vector3.up);
            }

            StartCoroutine(ActiveTime());
        }

    }



    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }

    protected override void OnEnable()
    {
        objRotation = Quaternion.Euler(0, -180, 0);
        base.OnEnable();
    }

    private void OnDisable()
    {
        
    }

    protected override IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        XbotPool.instance.ObjDisable(gameObject);

        
    }

    private void OnParticleCollision(GameObject other)
    {
        HP--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            HP -= other.GetComponent<BulletBase>().bulletInfo[2];
        }
    }


    /*
    /// <summary>
    /// 자동 비활성화
    /// </summary>
    /// <returns></returns>
    IEnumerator ActiveTime(float aliveTime = 0.0f)
    {
        
        
        XbotPool.instance.ObjDisable(gameObject);
    }
    */
}
