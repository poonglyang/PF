using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Ybot : MonoBehaviour
{
    /// <summary>
    /// 에니메이션 컴포넌트 변수
    /// </summary>
    Animator animator;

    /// <summary>
    /// 에니메이션 Action 해시
    /// </summary>
    int animatorAction;

    /// <summary>
    /// 레이케스트의 길이 사실상 총의 사거리와 같다.
    /// </summary>
    public float rayRange = 20f;

    /// <summary>
    /// 발사 속도
    /// </summary>
    public float shotInterval = 1.5f;

    /// <summary>
    /// ray 발사 지점
    /// </summary>
    Vector3 rayShotPos;

    /// <summary>
    /// 인풋 시스템 넣기
    /// </summary>
    PlayerInput playerInput;

    /// <summary>
    /// 오른쪽 방향 스케일 변경용
    /// </summary>
    Vector3 rightSied = new Vector3(0.8f, 0.8f, 0.8f);

    /// <summary>
    /// 왼쪽 방향 스케일 변경용
    /// </summary>
    Vector3 leftSide = new Vector3(-0.8f, 0.8f, 0.8f);

    /// <summary>
    /// 이동 방향
    /// </summary>
    Vector3 movePoint;

    /// <summary>
    /// 오른쪽 이동시 movePoint 값
    /// </summary>
    Vector3 rightMove = new Vector3(1, 0, 0);

    /// <summary>
    /// 왼쪽 이동시 movePoint 값
    /// </summary>
    Vector3 leftMove = new Vector3(-1, 0, 0);

    /// <summary>
    /// 직진시 movePoint 값
    /// </summary>
    Vector3 moveFoward = new Vector3(0, 0, 0);

    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody rig;

    /// <summary>
    /// 좌우 이동 속도
    /// </summary>
    public float moveSideSpeed = 2f;

    /// <summary>
    /// 배치를 담당할 플레이어메니저
    /// </summary>
    PlayerManager player;

    /// <summary>
    /// 플레이어 메니저에 등록된 YBot의 번호
    /// </summary>
    int yBotNumber;

    [SerializeField]
    /// <summary>
    /// 발사할 종류
    /// </summary>
    int bulletType = 0;

    /// <summary>
    /// 총알 타입
    /// </summary>
    enum BulletType
    {
        Pistol,
        Shotgun,
        AR,
        Sniper
    }

    /// <summary>
    /// 아이템 먹을 시 보이기 위한 것
    /// </summary>
    Transform itemParent;

    /// <summary>
    /// 아이템 배열
    /// </summary>
    Transform[] itemTransform;

    /// <summary>
    /// 샷건 파티클
    /// </summary>
    ParticleSystem shotgunBulletPS;


    /// <summary>
    /// YBot 스폰시 나오는 파티클
    /// </summary>
    ParticleSystem[] spawnPS;

    /// <summary>
    /// 샷건 탄알 발사각
    /// </summary>
    float fireAngle = 5f;

    public Action<int> GetItem;

    private void Awake()
    {
        // 인풋 시스템 등록
        playerInput = new PlayerInput();

        // 에니메이터 컴포넌트 가져옴
        animator = GetComponent<Animator>();
        // 애니메이터 변수
        animatorAction = Animator.StringToHash("Action");

        // 리지드바디
        rig = GetComponent<Rigidbody>();

        // 메인이 될 플레이어
        player = transform.GetComponentInParent<PlayerManager>();

        // 아이템들이 들어있는 부모 오브젝트
        itemParent = GetComponentInChildren<JustFindItemParent>().transform;

        // 배열 정의
        itemTransform = new Transform[itemParent.childCount];

        // 배열에 정보 넣기
        for (int i = 0; i < itemParent.childCount; i++)
        {
            itemTransform[i] = itemParent.GetChild(i);
        }

        // 샷건 파티클 가져오기
        shotgunBulletPS = transform.GetChild(3).GetComponent<ParticleSystem>();

        // 스폰 파티클 배열 정의
        spawnPS = new ParticleSystem[transform.GetChild(4).childCount + 1];

        // 파티클들 가져 오기(본인 포함)
        spawnPS = transform.GetChild(4).GetComponentsInChildren<ParticleSystem>(true);

        GetItem?.Invoke((int)BulletType.Pistol);

        GameProgressManager.instance.fireRateValueInfoChange += () =>
        {
            Debug.Log($"fireRateChangeDelegate = GameProgressManager.instance.fireRateValueInfoChange; 로 설정한 델리게이트 발동");
            DelegateFireRate();
        };
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Kema.Arrow.performed += ArrowClick;
        playerInput.Kema.Arrow.canceled += ArrowCancled;

        

        bulletType = 0;
        StopAllCoroutines();
        StartCoroutine(FireBullet());

        player.ChangeYBotActive();

        bulletType = (int)BulletType.Pistol;

        animator.SetInteger(animatorAction, 1);

        ItemActive((int)BulletType.Pistol);

        SpawnEffect();
    }


    private void OnDisable()
    {
        playerInput.Kema.Arrow.canceled -= ArrowCancled;
        playerInput.Kema.Arrow.performed -= ArrowClick;

        playerInput.Disable();

        player.ChangeYBotActive();
    }

    private void ArrowCancled(InputAction.CallbackContext obj)
    {
        
        movePoint = obj.ReadValue<Vector2>();
        //Debug.Log(movePoint);
        transform.localScale = rightSied;
    }

    private void ArrowClick(InputAction.CallbackContext obj)
    {
        
        movePoint = obj.ReadValue<Vector2>();
        //Debug.Log(movePoint);

        if(movePoint == leftMove)
        {
            transform.localScale = leftSide;
        }
        else
        {
            transform.localScale = rightSied;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("BossSpell") || other.CompareTag("Barrel"))
        {
            YBotPool.instance.ObjDisable(gameObject);
        }

        // 어떠한 아이템을 먹었다고 PlayerManager에게 알려줌
        if (other.CompareTag("ARItem"))
        {
            Debug.Log("AR 아이템을 먹음");
            GetItem?.Invoke((int)BulletType.AR);
        }
        else if (other.CompareTag("PistolItem"))
        {
            GetItem?.Invoke((int)BulletType.Pistol);
            Debug.Log("권총 아이템을 먹음");
        }
        else if (other.CompareTag("ShotgunItem"))
        {
            GetItem?.Invoke((int)BulletType.Shotgun);
            Debug.Log("샷건 아이템을 먹음");
        }
        else if (other.CompareTag("SniperItem"))
        {
            GetItem?.Invoke((int)BulletType.Sniper);
            Debug.Log("스나 아이템을 먹음");

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("BossSpell")) {
            YBotPool.instance.ObjDisable(gameObject);
        }

        

    }

    public void ChangeItem(int itemNum)
    {
        Debug.Log($"changeItem으로 넘어온 itemNum : {itemNum}");

        switch (itemNum)
        {
            case (int)BulletType.Pistol:
                shotInterval = GameProgressManager.instance.FireRateValue[0];
                bulletType = itemNum;
                ItemActive(itemNum);
                animator.SetInteger(animatorAction, 1);
                StopAllCoroutines();
                StartCoroutine(FireBullet());
                break;

            case (int)BulletType.Shotgun:
                shotInterval = GameProgressManager.instance.FireRateValue[1];
                bulletType = itemNum;
                ItemActive(itemNum);
                animator.SetInteger(animatorAction, 0);
                StopAllCoroutines();
                StartCoroutine(FireBullet());
                break;

            case (int)BulletType.AR:
                Debug.Log("AR로 총 바꿈 활성화");
                shotInterval = GameProgressManager.instance.FireRateValue[2];
                bulletType = itemNum;
                ItemActive(itemNum);
                animator.SetInteger(animatorAction, 0);
                StopAllCoroutines();
                StartCoroutine(FireBullet());
                break;

            case (int)BulletType.Sniper:
                shotInterval = GameProgressManager.instance.FireRateValue[3];
                bulletType = itemNum;
                ItemActive(itemNum);
                animator.SetInteger(animatorAction, 0);
                StopAllCoroutines();
                StartCoroutine(FireBullet());
                break;
        }
        
    }
    
    void DelegateFireRate()
    {
        shotInterval = GameProgressManager.instance.FireRateValue[bulletType];
    }

    void ItemActive(int itemNum)
    {
        for(int i = 0; i < itemTransform.Length; i ++)
        {
            itemTransform[i].gameObject.SetActive(false);
        }
        itemTransform[itemNum].gameObject.SetActive(true);
    }

    void SpawnEffect()
    {
        for(int i = 0; i < spawnPS.Length; i ++)
        {
            spawnPS[i].Play();
        }
    }

    // 블릿이 rayShotPos에서 rayRange까지 rayInterval동안 이동해야함 
    IEnumerator FireBullet()
    {
        
        while (true)
        {
            //Vector3 arriveBulletPos = rayShotPos + new Vector3(0, 0, rayRange);

            //bullet.transform.position = Vector3.Lerp(bullet.transform.position, arriveBulletPos, Time.deltaTime);
            // 발사 속도를 담당함
            yield return new WaitForSeconds(shotInterval);
            rayShotPos = new Vector3(transform.position.x, 1f, transform.position.z + 1);

            switch (bulletType)
            {
                case (int)BulletType.Pistol:
                    PistolBulletPool.instance.SetActiveObject(rayShotPos);
                    break;
                case (int)BulletType.AR:
                    ARBulletPool.instance.SetActiveObject(rayShotPos);
                    break;
                case (int)BulletType.Shotgun:
                    //shotgunBulletPS.Play();
                    for(int i = -2; i < 3; i ++)
                    {

                        ShotgunBulletPool.instance.SetActiveObject(rayShotPos, new Vector3(0,fireAngle * i,0));
                    }
                    
                    break;
                case (int)BulletType.Sniper:
                    SniperBulletPool.instance.SetActiveObject(rayShotPos);
                    break;
            }
        }
    }
}
