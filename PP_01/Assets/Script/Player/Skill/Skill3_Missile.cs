using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill3_Missile : MonoBehaviour
{
    public Action isHit;

    // Start is called before the first frame update

    Vector3 enablePos = new Vector3(-5, 8, 0.3f);

    private void OnEnable()
    {
        transform.localPosition = enablePos;
    }

    void Update()
    {
        transform.Translate(20f * Time.deltaTime * Vector3.right, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("미사일이 무언가에 닿았음");
        isHit?.Invoke();
    }

    
}
