using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour
{
    CashUI cashUI;

    public float liftTime = 20f;

    /// <summary>
    /// 아이템의 이동속도
    /// </summary>
    float moveSpeed = 0f;

    float rotateSpeed = 30f;

    private void Awake()
    {
        cashUI = FindAnyObjectByType<CashUI>();
    }

    private void OnEnable()
    {
        moveSpeed = 0;
        StopAllCoroutines();
        StartCoroutine(Move());
        StartCoroutine(ActiveTime(liftTime));
    }

    protected IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        CashPool.instance.ObjDisable(gameObject);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ybot"))
        {
            GameManager.instance.Cash++;
            GameOverPanel.instance.cashCount++;
            StartCoroutine(ActiveTime());
        }
    }


    IEnumerator Move()
    {
        yield return new WaitForSeconds(1f);

        moveSpeed = 8f;

    }

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.back,Space.World);
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up,Space.Self);
    }
}
