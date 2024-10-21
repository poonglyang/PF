using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : ObjectPool
{
    public override void SetActiveObject(Vector3 spawnPoint, Vector3? eulerAngel = null)
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();

            //Debug.Log($"{Quaternion.Euler(eulerAngel.GetValueOrDefault())}");

            obj.transform.position = spawnPoint;
            obj.transform.rotation = Quaternion.Euler(eulerAngel.GetValueOrDefault());
            obj.SetActive(true);
        }
        else
        {
            PoolUp();

            SetActiveObject(spawnPoint);
        }
    }
}
