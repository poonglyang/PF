using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : BulletBase
{

    protected override void AwakePlus()
    {

        if (gameProgressManager == null)
        {
            gameProgressManager = FindAnyObjectByType<GameProgressManager>();
        }

        loadBulletInfo();

        gameProgressManager.psitolBulletInfoChange += () =>
        {
            loadBulletInfo();
        };
    }


    protected override void loadBulletInfo()
    {

        for (int i = 0; i < bulletInfo.Length; i++)
        {
            bulletInfo[i] = gameProgressManager.PistolBulletValue[i];
        }

        //Debug.Log($"bulletRange : {bulletInfo[(int)BulletInfoEnum.bulletRange]} bulletSpeed = {(int)BulletInfoEnum.bulletSpeed} bulletDamage = {(int)BulletInfoEnum.bulletDamage}");
    }

    protected override IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        PistolBulletPool.instance.ObjDisable(gameObject);
    }
}
