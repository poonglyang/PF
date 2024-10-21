using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2_KillEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            //XbotPool.instance.ObjDisable(other.gameObject);

            other.GetComponent<EnemyBase>().HP -= 30f;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            //XbotPool.instance.ObjDisable(collision.gameObject.gameObject);
            collision.gameObject.GetComponent<EnemyBase>().HP -= 30f;
        }
    }
}
