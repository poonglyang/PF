using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Barrel : EnemyBase
{
    /// <summary>
    /// 실제 보이는 베럴 오브젝트
    /// </summary>
    Transform barrelModel;

    [Header("barrel stat")]
    /// <summary>
    /// 베럴의 회전 속도
    /// </summary>
    public float barrelRollSpeed = 200f;

    /// <summary>
    /// 베럴 텍스트
    /// </summary>
    TextMeshPro berralText;

    /// <summary>
    /// 아이템 통의 hp가 0이 되었을 경우 폭발 효과 소환을 위한 프리팹
    /// </summary>
    public GameObject explode;

    public GameObject[] itemObjcet;

    /// <summary>
    /// 아이템 번호
    /// </summary>
    int item;


    protected override void Awake()
    {
        barrelModel = gameObject.transform.GetChild(0);
        berralText = gameObject.transform.GetChild(1).GetComponent<TextMeshPro>();
        HP += 2f;
        berralText.text = HP.ToString("f0");

        Transform itemParent = transform.GetChild(2);

        item = Random.Range(0, itemParent.childCount);

        itemParent.GetChild(item).gameObject.SetActive(true);

    }

    protected override void OnEnable()
    {
        // 시작할 떄 반경 n안에 
        //DisableObject();
        //DisableObject();

        base.OnEnable();

        StartCoroutine(DisableTimer(1.5f));
    }



    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.back);
        barrelModel.Rotate(0, barrelRollSpeed * Time.deltaTime, 0);
    }

    private void OnParticleCollision(GameObject other)
    {
        HP--;
        StartCoroutine(HitEffect());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            HP -= other.GetComponent<BulletBase>().bulletInfo[2];
            StartCoroutine(HitEffect());
        }
    }

    protected override void WhenHit()
    {
        berralText.text = HP.ToString("f0");
        if(HP < 1)
        {
            spawnExplodeEffect();
            Instantiate(itemObjcet[item], transform.position, Quaternion.identity) ;
            StartCoroutine(ActiveTime());
        }
    }

    void spawnExplodeEffect()
    {
        Instantiate(explode, transform.position, Quaternion.identity);
    }

    protected override IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        BarrelPool.instance.ObjDisable(gameObject);
    }


    IEnumerator HitEffect()
    {
        transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);

        yield return new WaitForSeconds(0.1f);

        transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
    }

}
