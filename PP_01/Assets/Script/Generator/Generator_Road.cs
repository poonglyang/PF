using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator_Road : Generator_Base
{
    /// <summary>
    /// 첫 번째 Road 좌표
    /// </summary>
    Vector3 spawnBasePos = new Vector3(0, 0, -4);

    /// <summary>
    /// 다음 Road 생성 시 더해주는 좌표
    /// </summary>
    Vector3 addPos = new Vector3(0, 0, 16f);

    /// <summary>
    /// 생성 좌표
    /// </summary>
    Vector3 spawnPoint;

    /// <summary>
    /// 이름 변경용
    /// </summary>
    GameObject[] Road;

    public float RoadMoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
        }
    }

    private void Awake()
    {
        Road = new GameObject[12];
        spawnPoint = spawnBasePos;          // 스폰 위치 베이스
        for (int i = 0; i < 12; i++)
        {
            Road[i] = Instantiate(prefab, spawnPoint, Quaternion.identity);    // 생성 (나중에 바꿀 것)
            Road[i].transform.parent = transform;                              
            Road[i].name = $"Road_{i}";                                        // 이름 지정
            spawnPoint += addPos + new Vector3(0,0,0.001f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Road.Length; i ++)
        {
            Road[i].transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);

            if (Road[i].transform.position.z < -20)
            {
                Road[i].transform.position = new Vector3(0, 0, 92);
            }
        }
        
    }
}
