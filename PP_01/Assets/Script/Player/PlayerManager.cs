using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// 인풋 시스템 넣기
    /// </summary>
    PlayerInput playerInput;

    /// <summary>
    /// 이동 방향
    /// </summary>
    Vector3 movePoint;

    /// <summary>
    /// yBot 최대 개수
    /// </summary>
    public int yBotMax = 9;

    /// <summary>
    /// 델리게이트를 위한 Ybot 컴포넌트 가져오기
    /// </summary>
    Ybot[] yBotDelegate;

    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody rig;

    /// <summary>
    /// 좌우 이동 속도
    /// </summary>
    public float moveSideSpeed = 3f;

    /// <summary>
    /// 현재 들어있는 Ybot개수
    /// </summary>
    int yBotNum = 0;

    /// <summary>
    /// 현재 들어있는 Ybot의 컴포넌트
    /// </summary>
    Ybot[] testYbot;

    /// <summary>
    /// 보더의 위치값이다
    /// </summary>
    Vector3[,] borderPos = {
        { new Vector3(3.5f, 0.3f, 0), new Vector3(-3.5f, 0.3f, 0) },
        { new Vector3(3f, 0.3f, 0), new Vector3(-3f, 0.3f, 0) }
    };

    /// <summary>
    /// YBot 개수에 따라 보더의 위치를 변화하기 위한 border 찾기
    /// </summary>
    Border border;

    /// <summary>
    /// 오른쪽 border의 boxCollider.center를 바꾸기 위한 것
    /// </summary>
    BoxCollider rightBorder;

    /// <summary>
    /// 왼쪽 border의 boxCollider.center를 바꾸기 위한 것
    /// </summary>
    BoxCollider leftBorder;

    /// <summary>
    /// 이팩트 소환을 위한 이전 Ybot의 수
    /// </summary>
    int oldYbotCount = 0;

    /// <summary>
    /// 무적 시간
    /// </summary>
    public float invincibilityTime = 1f;




    /// <summary>
    /// 시작했음을 알림
    /// </summary>
    bool isStart = false;

    /// <summary>
    /// 죽었음을 알리는 델리게이트
    /// </summary>
    public Action onDie;

    float[] bulletStatsModifiers = new float[] { 1.2f, 1.2f, 0.6f, -0.1f, 0.7f, 0.8f, 0.3f, -0.08f, 3f, 1f, 1.1f, -0.15f, 7f, 4f, 4f, -0.05f };

    private void Awake()
    {
        playerInput = new PlayerInput();

        //// ybot 가져오기
        //yBotDelegate = GetComponentsInChildren<Ybot>(true);

        //for(int i = 0; i < yBotDelegate.Length; i ++)
        //{
        //    // 델리게이트 부착
        //    yBotDelegate[i].GetItem += RandomYbotGunChange;
        //}

            

        // 리지드바디
        rig = GetComponent<Rigidbody>();

        Debug.Log("Awake 실행");

        border = GameObject.FindAnyObjectByType<Border>();
        rightBorder = border.gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider>();
        leftBorder = border.gameObject.transform.GetChild(1).gameObject.GetComponent<BoxCollider>();

        StartCoroutine(StartDelay());

    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * moveSideSpeed * movePoint);

    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Kema.Arrow.performed += ArrowClick;
        playerInput.Kema.Arrow.canceled += ArrowCancled;

    }

    private void OnDisable()
    {

        playerInput.Kema.Arrow.canceled -= ArrowCancled;
        playerInput.Kema.Arrow.performed -= ArrowClick;

        playerInput.Disable();
    }

    private void ArrowCancled(InputAction.CallbackContext obj)
    {
        movePoint = obj.ReadValue<Vector2>();
    }

    private void ArrowClick(InputAction.CallbackContext obj)
    {
        movePoint = obj.ReadValue<Vector2>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pannal"))
        {
            int creaseRate = other.gameObject.GetComponent<Pannal>().creaseRate;

            if (creaseRate > 0)
            {
                if (testYbot.Length != 9)
                {
                    for (int i = 0; i < creaseRate; i++)
                    {
                        YBotPool.instance.SetActiveObject(new Vector3(0, 0.1f, 0));
                    }
                }
            }
            else
            {
                for(int i = 0; i < Mathf.Abs(creaseRate); i ++)
                {
                    if (testYbot.Length != 0)
                    {
                        YBotPool.instance.ObjDisable(testYbot[Random.Range(0, testYbot.Length)].gameObject);
                    }
                }
            }
        }
    }

    /// <summary>
    /// PlayerManager는 범위 안에 있는데 YBot는 그대로 있는 문제로 인해 작성함
    /// 더 좋은 방법이 있을 시(Stay를 안쓰는 방법) 그것을 사용할 예정
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Border"))
            ReplaceYbot();
    }

    /// <summary>
    /// YBot의 개수에 변화가 생김
    /// </summary>
    public void ChangeYBotActive()
    {
        testYbot = GetComponentsInChildren<Ybot>(false);

        for (int i = 0; i < testYbot.Length; i++)
        {
            // 델리게이트 부착
            testYbot[i].GetItem += RandomYbotGunChange;
        }

        YBotArrayLength();
        ReplaceYbot();

        if(isStart && testYbot.Length < 1)
        {
            onDie?.Invoke();
            
        }

        if(testYbot.Length > 0 && gameObject.activeSelf && isStart)
        {
            StartCoroutine(YbotInvincibility(invincibilityTime));
        }
        
    }

    /// <summary>
    /// 현재 활성화된 YBot의 개수에 따라 YBot들의 위치를 재조정한다
    /// </summary>
    void ReplaceYbot()
    {
        float yBotPosY = 0.1f;
        float botInterval = 0.5f;

        // testYbot.Length을 쓰는 일이 많아 따로 빼 두었다
        int yBosArrayLength = testYbot.Length;

        ReplaceBorder(yBosArrayLength);

        switch (yBosArrayLength)
        {

            case 1:
                testYbot[0].transform.position = transform.position;
                break;
            case 2:
                testYbot[0].transform.position = new Vector3(transform.position.x - 0.25f, yBotPosY, 0);
                testYbot[1].transform.position = new Vector3(transform.position.x + 0.25f, yBotPosY, 0);
                break;
            case 3:
                for (int i = -1; i < 2; i++)
                {
                    testYbot[i + 1].transform.position = new Vector3(transform.position.x + botInterval * i, yBotPosY, 0);
                }
                break;
            case 4:
                for (int i = -1; i < 2; i++)
                {
                    testYbot[i + 1].transform.position = new Vector3(transform.position.x + botInterval * i, yBotPosY, 0);
                }
                testYbot[3].transform.position = new Vector3(transform.position.x, yBotPosY, 1);
                break;
            case 5:
                for (int i = -2; i < 3; i++)
                {
                    if (i < 0)
                    {
                        testYbot[i + 2].transform.position = new Vector3(transform.position.x + botInterval * i + 0.25f, yBotPosY, 0);
                    }
                    else if (i > 0)
                    {
                        testYbot[i + 2].transform.position = new Vector3(transform.position.x + botInterval * i - 0.25f, yBotPosY, 0);
                    }
                    else
                    {
                        testYbot[i + 2].transform.position = new Vector3(transform.position.x, yBotPosY, 1);
                    }
                }
                break;
            case 6:
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        testYbot[3 * i + j].transform.position = new Vector3(transform.position.x + (j - 1) * botInterval, yBotPosY, i);
                    }
                }
                break;
            case 7:
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        testYbot[3 * i + j].transform.position = new Vector3(transform.position.x + (j - 1) * botInterval, yBotPosY, i);
                    }
                }
                testYbot[6].transform.position = new Vector3(transform.position.x, yBotPosY, 2);
                break;
            case 8:
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (j < 2)
                        {
                            testYbot[4 * i + j].transform.position = new Vector3(transform.position.x + (j - 2) * botInterval, yBotPosY, i);
                        }
                        else
                        {
                            testYbot[4 * i + j].transform.position = new Vector3(transform.position.x + (j - 1) * botInterval, yBotPosY, i);
                        }

                    }
                }
                break;
            case 9:
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        testYbot[3 * i + j].transform.position = new Vector3(transform.position.x + (j - 1) * botInterval, yBotPosY, i);
                    }
                }
                break;

        }
    }

    /// <summary>
    /// ybot을 무적으로 만들어주는 코루틴을 실행하는 함수
    /// </summary>
    public void Ybotinv(float invinTime)
    {
        StartCoroutine(YbotInvincibility(invinTime));
    }

    /// <summary>
    /// 무적으로 바꿔주는 코루틴
    /// </summary>
    /// <param name="invinTime">무적시간</param>
    /// <returns></returns>
    IEnumerator YbotInvincibility(float invinTime)
    {

        for (int i =0; i < testYbot.Length; i ++)
        {
            testYbot[i].gameObject.layer = 12;
        }


        yield return new WaitForSeconds(invinTime);

        for (int i = 0; i < testYbot.Length; i++)
        {
            testYbot[i].gameObject.layer = 9;
        }

    }

    /// <summary>
    /// Ybot의 개수에 따라 보더의 위치를 조정한다
    /// </summary>
    /// <param name="yBotArrayLength">Ybot의 개수</param>
    void ReplaceBorder(int yBotArrayLength)
    {
        

        if(rightBorder != null && leftBorder != null)
        {
            if (yBotArrayLength == 5 || yBotArrayLength == 8)
            {
                rightBorder.center = borderPos[1, 0];
                leftBorder.center = borderPos[1, 1];
            }
            else
            {
                rightBorder.center = borderPos[0, 0];
                leftBorder.center = borderPos[0, 1];
            }
        }
        else
        {
            Debug.Log("Border가 없는 상황에서 참조하려고 함");
        }
            

        
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1);
        YBotPool.instance.SetActiveObject(new Vector3(0, 0.1f, 0));
        isStart = true;
    }

    void YBotArrayLength()
    {
        if(testYbot.Length > oldYbotCount)
        {
            //SpawnEffectPool.instance.SetActiveObject(transform.position);
        }

        oldYbotCount = testYbot.Length;

    }

    /// <summary>
    /// Ybot중 하나가 item을 먹었을 때 랜덤 Ybot의 아이템을 그 아이템으로 바꿈
    /// </summary>
    /// <param name="itemNum">먹은 아이템</param>
    void RandomYbotGunChange(int itemNum)
    {
        int changItemYbot = Random.Range(0, testYbot.Length);

        Debug.Log($"아이템을 바꾼 ybot{changItemYbot}, 아이템 {itemNum}");

        IncreaseItemPower(itemNum);

        testYbot[changItemYbot].ChangeItem(itemNum);

    }

    void IncreaseItemPower(int itemNum)
    {
        int increaseAbility = Random.Range(1, 4);

        float increaseValue = bulletStatsModifiers[itemNum * 4 + increaseAbility];

        if (increaseAbility == 3)
        {
            GameProgressManager.instance.FireRateValue[itemNum] += increaseValue;
        }
        else
        {
            switch (itemNum)
            {
                case 0:
                    GameProgressManager.instance.PistolBulletValue[increaseAbility] += increaseValue;
                    break;
                case 1:
                    GameProgressManager.instance.ShotgunBulletValue[increaseAbility] += increaseValue;
                    break;
                case 2:
                    GameProgressManager.instance.ARBulletValue[increaseAbility] += increaseValue;
                    break;
                case 3:
                    GameProgressManager.instance.SniperBulletValue[increaseAbility] += increaseValue;
                    break;
            }
        }
    }
}


