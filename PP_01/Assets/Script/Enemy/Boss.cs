using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Boss : EnemyBase
{
    [Header("보스 관련 변수")]
    /// <summary>
    /// 애니메이터 컴포넌트
    /// </summary>
    Animator animator;

    /// <summary>
    /// 동작
    /// </summary>
    readonly int bossAniHash = Animator.StringToHash("Action");

    [Header("보스의 데이터")]
    /// <summary>
    /// 좀비 소환 패턴으로 소환할 좀비
    /// </summary>
    public int spawnZombCount = 10;

    /// <summary>
    /// 좀비 스폰 패턴의 이팩트
    /// </summary>
    ParticleSystem[] ZombSpawnPS;

    /// <summary>
    /// 스팰 발사 간격
    /// </summary>
    public  float spellInterval = 1.5f;

    /// <summary>
    /// 마법을 쏠 위치
    /// </summary>
    Transform spellTransform;

    /// <summary>
    /// 플레이어 메니저의 위치로 스팰을 쏨
    /// </summary>
    Transform playerManagerTransform;

    /// <summary>
    /// 마법 발사 코루틴
    /// </summary>
    IEnumerator SpellCoroutine;

    /// <summary>
    /// 스팰 캐스트 ps
    /// </summary>
    ParticleSystem spellCastPS;

    /// <summary>
    /// hp바 이미지
    /// </summary>
    Image[] hpBar;

    /// <summary>
    /// 최대 hp
    /// </summary>
    public float MaxHP = 100;

    /// <summary>
    /// 보스 hp를 나타내는 택스트
    /// </summary>
    TextMeshPro bossHP;

    /// <summary>
    /// 보스 사망 파티클의 부모
    /// </summary>
    Transform bossDieParentTransform;

    /// <summary>
    /// 보스 사망 파티클
    /// </summary>
    ParticleSystem[] bossDiePS;

    /// <summary>
    /// 패턴 간격
    /// </summary>
    public float patternInterval = 15f;

    /// <summary>
    /// 발동할 패턴의 개수 패턴을 다 발동하면 n초뒤 전멸기가 나온다
    /// </summary>
    public int patternCount = 5;

    /// <summary>
    /// 위의 것의 n
    /// </summary>
    public float timeOutT = 5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        ZombSpawnPS = transform.GetChild(7).GetComponentsInChildren<ParticleSystem>();

        //animator.GetCurrentAnimatorClipInfo(0); //컨트롤러의 첫번째 레이어가 가지고 있는 클립 정보 가져오기
        //AnimatorClipInfo info = animator.GetCurrentAnimatorClipInfo(0)[0]; //컨트롤러의 첫번째 레이어의 첫번째 클립의 정보 가져오기

        //for (int i =0; i < animator.GetCurrentAnimatorClipInfo(0).Length; i ++)
        //{
        //    Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[i].clip.name);
        //}

        playerManagerTransform = FindAnyObjectByType<PlayerManager>().transform;

        spellTransform = transform.GetChild(8);

        SpellCoroutine = ShootSpell();

        spellCastPS = spellTransform.GetComponentInChildren<ParticleSystem>();

        hpBar = transform.GetChild(9).GetChild(0).GetComponentsInChildren<Image>();

        bossHP = GetComponentInChildren<TextMeshPro>();

        bossDieParentTransform = transform.GetChild(11);

        bossDiePS = bossDieParentTransform.GetComponentsInChildren<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        moveSpeed = 0;
        HP = MaxHP;

        for (int i = 0; i < ZombSpawnPS.Length; i++)
        {
            ZombSpawnPS[i].Stop();
        }

        spellCastPS.Stop();

        for (int i = 0; i < bossDiePS.Length; i++)
        {
            bossDiePS[i].Stop();
        }

        for (int i = 0; i < hpBar.Length; i++)
        {
            hpBar[i].gameObject.SetActive(true);
        }

        gameObject.layer = 6;

        bossHP.gameObject.SetActive(true);

        StartCoroutine(BossPatternStart());
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

    protected override void WhenHit()
    {
        hpBar[1].fillAmount = HP/MaxHP;
        bossHP.text = HP.ToString("F0");

        if(HP < 1)
        {
            bossHP.gameObject.SetActive(false);

            for(int i = 0; i < hpBar.Length; i ++)
            {
                hpBar[i].gameObject.SetActive(false);
            }

            for(int i = 0; i < bossDiePS.Length; i++)
            {
                bossDiePS[i].Play();
            }

            gameObject.layer = 12;

            GameOverPanel.instance.bossZombDieCount++;
            if (Random.value < goodsDrop)
            {
                CashPool.instance.SetActiveObject(transform.position + Vector3.up);
            }

            StartCoroutine(ActiveTime(1f));
        }
    }

    /// <summary>
    /// 타임오버 패턴 발동
    /// </summary>
    void TimeOut()
    {
        StartCoroutine(TimeOutPattern());
        
    }

    /// <summary>
    /// 보스 타임 오버 패턴 시작
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeOutPattern()
    {
        animator.SetInteger(bossAniHash, 0);

        while (transform.position.z - playerManagerTransform.position.z > 2.5)
        {
            moveSpeed = 4;

            yield return null;
        }

        moveSpeed = 0;

        animator.SetInteger(bossAniHash, 3);

        yield return new WaitForSeconds(2f);

        animator.SetInteger(bossAniHash, 0);

        StartCoroutine(ActiveTime(10f));

    }

    /// <summary>
    /// 보스가 낫을 휘두른 뒤 빠져 나가기 위한 moveSpeed 변경
    /// </summary>
    void AfterAtack()
    {
        moveSpeed = 2;
    }

    /// <summary>
    /// 낫을 휘두를 때 모든 플레이어 봇 비활성화
    /// </summary>
    void KillAllYBot()
    {
        for (int i = 0; i < playerManagerTransform.childCount; i++)
        {
            YBotPool.instance.ObjDisable(playerManagerTransform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 좀비를 소환하는 패턴
    /// </summary>
    void SpawnZomb()
    {
        // 6.12초간 진행
        float spawnInterval = 4.12f / spawnZombCount;
        animator.SetInteger(bossAniHash, 1);

        StartCoroutine(ZombSpawnPattern(spawnInterval));

        StartCoroutine(ZombSpawnEffect());

    }

    /// <summary>
    /// 일정 시간 간격으로 좀비 소환
    /// </summary>
    /// <param name="spawnInterval">좀비 소환 간격</param>
    /// <returns></returns>
    IEnumerator ZombSpawnPattern(float spawnInterval)
    {
        Debug.Log("좀비 스폰 패턴 실행");

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < spawnZombCount; i ++)
        {
            yield return new WaitForSeconds(spawnInterval);

            float spawnPosX = Random.Range(-3f, 3f);
            float spawnPosZ = Random.Range(-3f, 3f);

            XbotPool.instance.SetActiveObject(new Vector3(transform.position.x + spawnPosX, 0, transform.position.z + spawnPosZ));
        }
        animator.SetInteger(bossAniHash, 0);
    }

    /// <summary>
    /// 소환 이팩트
    /// </summary>
    /// <returns></returns>
    IEnumerator ZombSpawnEffect()
    {
        yield return new WaitForSeconds(2f);
        for(int i =0; i < ZombSpawnPS.Length; i ++)
        {
            ZombSpawnPS[i].Play();
        }
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < ZombSpawnPS.Length; i++)
        {
            ZombSpawnPS[i].Stop();
        }
    }

    void Spell()
    {
        animator.SetInteger(bossAniHash, 2);
    }

    /// <summary>
    /// 어두운 구체 파티클을 난사하는 패턴
    /// </summary>
    void StartSpell()
    {
        spellCastPS.Play();
        StartCoroutine(SpellCoroutine);
    }

    IEnumerator ShootSpell()
    {
        while(true)
        {
            yield return new WaitForSeconds(spellInterval);

            BossSpellEffectPool.instance.SetActiveObject(spellTransform.position);
        }
    }

    void StopSpell()
    {
        spellCastPS.Stop();
        StopCoroutine(SpellCoroutine);
        animator.SetInteger(bossAniHash, 0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            SpawnZomb();

        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            TimeOut();
        }

        if(Input.GetKeyDown(KeyCode.D)) {
            Spell();
        }

        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }

    IEnumerator BossPatternStart()
    {

        for(int i = 0; i < patternCount; i ++)
        {
            Debug.Log($"패턴 실행{i}");
            if(Random.value < 0.5f)
            {
                SpawnZomb();
            }
            else
            {
                Spell();
            }
            yield return new WaitForSeconds(patternInterval);

        }

        yield return new WaitForSeconds(patternInterval);

        TimeOut();


    }

    protected override IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        BossPool.instance.ObjDisable(gameObject);
    }

    
}
