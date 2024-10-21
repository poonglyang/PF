using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    //DontDestroyOnLoad(gameObject);

    public GameObject prefab;

    protected Queue<GameObject> poolQueue;

    /// <summary>
    /// 생성할 개수
    /// </summary>
    public int GenerateValue = 64;

    /// <summary>
    /// 64개 미리 생성
    /// </summary>
    protected virtual void Awake()
    {
        
        poolQueue = new Queue<GameObject>(GenerateValue);

        CreateQueue(0, GenerateValue, ref poolQueue);
    }

    /// <summary>
    /// 큐 생성, 오브젝트 풀을 처음 만들거나 풀 이상의 것을 생성해야 할 때 실행
    /// </summary>
    /// <param name="start"></param>
    /// <param name="j"></param>
    /// <param name="pool"></param>
    void CreateQueue(int start, int j, ref Queue<GameObject> pool)
    {
        for (int i = start; i < j; i++)
        {
            GameObject obj = Instantiate(prefab, transform);    // 풀의 자식으로 생성
            obj.name = $"{prefab.name}_{i}";                    // 이름 변경

            pool.Enqueue(obj);
            
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 일반 오브젝트 불러오기
    /// </summary>
    public virtual void SetActiveObject (Vector3 spawnPoint, Vector3? eulerAngel = null)
    {
        if(poolQueue.Count > 0)
        {
            GameObject obj =  poolQueue.Dequeue();

            //Debug.Log($"{Quaternion.Euler(eulerAngel.GetValueOrDefault())}");
            
            obj.transform.position = spawnPoint;
            //obj.transform.rotation = Quaternion.Euler(eulerAngel.GetValueOrDefault());
            obj.SetActive(true);
        }
        else
        {
            PoolUp();

            SetActiveObject(spawnPoint);
        }
    }


    /// <summary>
    /// 풀 사이즈 증가
    /// </summary>
    protected void PoolUp()
    {
        int newPoolSize = GenerateValue * 2;

        Queue<GameObject> newQueue = new Queue<GameObject>(newPoolSize);

        newQueue = poolQueue;

        CreateQueue(GenerateValue, newPoolSize, ref newQueue);

        GenerateValue = newPoolSize;

        poolQueue = newQueue;
    }

    public virtual void ObjDisable(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
    }
    

}
