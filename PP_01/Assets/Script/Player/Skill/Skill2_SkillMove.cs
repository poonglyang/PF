using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2_SkillMove : MonoBehaviour
{
    public float moveSpeed = 10f;

    // Start is called before the first frame update
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ActiveTime(5f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }

    IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        SkillAriRaidPool.instance.ObjDisable(gameObject);
    }
}
