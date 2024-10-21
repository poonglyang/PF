using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : BulletBase
{
    protected override void AwakePlus()
    {

        if (gameProgressManager == null)
        {
            gameProgressManager = FindAnyObjectByType<GameProgressManager>();
        }

        loadBulletInfo();

        gameProgressManager.shotgunBulletInfoChange += () =>
        {
            loadBulletInfo();
        };
    }

    protected override void UpdatePlus()
    {
        transform.Translate(0, 0, bulletInfo[(int)BulletInfoEnum.bulletSpeed] * Time.deltaTime, Space.Self);
    }


    protected override void loadBulletInfo()
    {

        for (int i = 0; i < bulletInfo.Length; i++)
        {
            bulletInfo[i] = gameProgressManager.ShotgunBulletValue[i];
        }

        //Debug.Log($"bulletRange : {bulletInfo[(int)BulletInfoEnum.bulletRange]} bulletSpeed = {(int)BulletInfoEnum.bulletSpeed} bulletDamage = {(int)BulletInfoEnum.bulletDamage}");
    }

    protected override IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        //현재 샷건은 풀이 없음
        ShotgunBulletPool.instance.ObjDisable(gameObject);
    }
}
