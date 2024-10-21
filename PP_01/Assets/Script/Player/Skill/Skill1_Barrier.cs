using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1_Barrier : MonoBehaviour
{

    Transform player;

    readonly Vector3[] shildPos = new Vector3[3]
    {
        new Vector3(0, 1, 1.5f),
        new Vector3(0, 1, 2f),
        new Vector3(0, 1, 2.5f)
    };

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerManager>().GetComponent<Transform>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ActiveTime(3f));
    }

    private void Update()
    {
        Vector3 playerPos = player.position;

        int playerChild = GetActiveChildCount(player);

        

        if (playerChild < 5)
        {
            Debug.Log(playerChild);
            transform.position = playerPos + shildPos[0];
        } 
        else if (playerChild < 8)
        {
            Debug.Log(playerChild);
            transform.position = playerPos + shildPos[1];
        }
        else if (playerChild < 11)
        {
            Debug.Log(playerChild);
            transform.position = playerPos + shildPos[2];
        }


    }

    int GetActiveChildCount(Transform parent)
    {
        int activeChildCount = 0;

        // 부모 Transform의 모든 자식 순회
        foreach (Transform child in parent)
        {
            // 자식 오브젝트가 활성화되어 있는지 확인
            if (child.gameObject.activeSelf)
            {
                activeChildCount++;
            }
        }

        return activeChildCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Barrel"))
        {
            other.GetComponent<EnemyBase>().HP -= 100f;
        }
        else if(other.CompareTag("BossSpell"))
        {
            BossSpellEffectPool.instance.ObjDisable(other.gameObject);
        }
    }

    IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        SkillBarrierPool.instance.ObjDisable(gameObject);
    }
}
