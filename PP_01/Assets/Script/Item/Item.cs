using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// 리지드바디 컴포넌트
    /// </summary>
    Rigidbody rigid;

    /// <summary>
    /// 아이템의 이동속도
    /// </summary>
    float moveSpeed = 0f;

    Collider collider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.AddForce(new Vector3(0, 5, 5), ForceMode.VelocityChange);

        collider = GetComponent<Collider>();

        StartCoroutine(Move());

        Destroy(gameObject, 20f);

    }


    IEnumerator Move()
    {
        yield return new WaitForSeconds(1.5f);

        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        rigid.useGravity = false;
        collider.isTrigger = true;

        moveSpeed = 8f;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ybot"))
        {
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ybot"))
        {
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.back);
    }
}
