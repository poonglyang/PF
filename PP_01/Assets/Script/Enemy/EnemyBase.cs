using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy default stat")]

    /// <summary>
    /// 이동속도, 기본 값 3
    /// </summary>
    public float moveSpeed = 3f;

    /// <summary>
    /// 생존 시간
    /// </summary>
    public float liftTime = 25f;

    public float hp = 1;

    /// <summary>
    /// 탐지 범위
    /// </summary>
    public float detectionEnemyArea = 4f;

    public Quaternion objRotation = Quaternion.Euler(0, 0, 0); 

    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            WhenHit();
        }
    }

    /// <summary>
    /// 코인이 나올 확률
    /// </summary>
    public float goodsDrop = 0.8f;
    protected virtual void Awake()
    {
        StartCoroutine(ActiveTime(liftTime));
        AwakePlus();
    }

    protected virtual void AwakePlus()
    {

    }

    protected virtual void OnEnable()
    {
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = objRotation;
        StopAllCoroutines();
        StartCoroutine(ActiveTime(liftTime));
    }


    protected virtual void WhenHit()
    {

    }

    /// <summary>
    /// 비활성하 시키는 코루틴
    /// </summary>
    /// <param name="aliveTime"></param>
    /// <returns></returns>
    protected virtual IEnumerator ActiveTime(float aliveTime = 0.0f)
    {
        yield return new WaitForSeconds(aliveTime);
        //Debug.Log("비활성화 실행");

        // 삭제 예정 오버라이드 하고 작성할 것
        //XbotPool.instance.ObjDisable(gameObject);
    }

    /*------------------ 기존 Barrel 스크립트에 있었다가 옮김 -------------------*/

    /// <summary>
    /// detectionEnemyArea 만큼 주변 적 비활성화
    /// </summary>
    protected virtual void DisableObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionEnemyArea);
        foreach (var hitCollider in hitColliders)
        {
            //Debug.Log(hitCollider.gameObject);
            // 태그가 Enemy가 있으면 삭제
            if (hitCollider.CompareTag("Enemy"))
            {
                //Debug.Log($"겹치는 적 삭제{hitCollider.name}");
                XbotPool.instance.ObjDisable(hitCollider.gameObject);
            }
        }
    }

    

    /// <summary>
    /// 비활성화 할 범위 그리기
    /// </summary>
    void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, detectionEnemyArea);
    }

    protected IEnumerator DisableTimer(float setTime)
    {
        yield return new WaitForSeconds(setTime);
        DisableObject();
    }


}
