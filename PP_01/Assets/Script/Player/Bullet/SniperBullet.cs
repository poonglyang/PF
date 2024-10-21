using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : BulletBase
{
    public int penetrate = 3;

    protected override void AwakePlus()
    {

        if(gameProgressManager == null)
        {
            gameProgressManager = FindAnyObjectByType<GameProgressManager>();
        }

        loadBulletInfo();

        gameProgressManager.sniperBulletInfoChange += () =>
        {
            loadBulletInfo();
        };
    }


    protected override void loadBulletInfo()
    {

        for(int i = 0; i < bulletInfo.Length; i ++)
        {
            bulletInfo[i] = gameProgressManager.SniperBulletValue[i];
        }

        //Debug.Log($"bulletRange : {bulletInfo[(int)BulletInfoEnum.bulletRange]} bulletSpeed = {(int)BulletInfoEnum.bulletSpeed} bulletDamage = {(int)BulletInfoEnum.bulletDamage}");
    }

    protected override void OnTriggerEnter(Collider other)
    {
        penetrate--;

        if(penetrate == 0)
        {
            StartCoroutine(ActiveTime());
        }
        
    }

    protected override IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        SniperBulletPool.instance.ObjDisable(gameObject);
    }


}
