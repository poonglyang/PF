using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemyMultyGenerator : MonoBehaviour
{
    
    
    public enum EnemyObj
    {
        EnemyBot,
        BarrelAndPannal,
        Boss
    }
    
    [Serializable]
    public struct EnemyObject
    {
        public EnemyObj obj;
        public float spawnTime;
    }

    public EnemyObject[] enemyObject;

    float barrelAndPannalSpawnPosX = 2.25f;

    public GameObject bossSpawnEffect;

    Vector3 bossSpawnPos = new Vector3(0, 0, 16);

    private void Awake()
    {

        foreach (var enemy in enemyObject)
        {
            StartCoroutine(SpawnCoroutine(enemy));
        }
    }
    
    IEnumerator SpawnCoroutine(EnemyObject enemyobj)
    {
        while(true)
        {
            yield return new WaitForSeconds(enemyobj.spawnTime);
            switch(enemyobj.obj)
            {
                case EnemyObj.EnemyBot:
                    XbotPool.instance.SetActiveObject(new Vector3(Random.Range(-3.5f, 3.5f), 0, transform.position.z));
                    break;
                case EnemyObj.BarrelAndPannal:
                    int spawnObj = Random.Range(1, 8);
                    
                    for(int i = -1; i < 2; i ++)
                    {
                        if(Convert.ToBoolean(Random.Range(0, 2)))
                        {
                            if(Random.value < 0.5)
                            {
                                BarrelPool.instance.SetActiveObject(new Vector3(barrelAndPannalSpawnPosX * i, 0.5f, transform.position.z));
                            }
                            else
                            {
                                PannalPool.instance.SetActiveObject(new Vector3(barrelAndPannalSpawnPosX * i, 0, transform.position.z));
                            }
                        }
                    }
                    
                    break;
                case EnemyObj.Boss:
                    StartCoroutine(BossSpawn());
                    break;
            }
        }
    }

    IEnumerator BossSpawn()
    {
        Instantiate(bossSpawnEffect, bossSpawnPos, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        BossPool.instance.SetActiveObject(bossSpawnPos);
    }

    
}
