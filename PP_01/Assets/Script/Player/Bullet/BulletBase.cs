using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{

    protected enum BulletInfoEnum{
        bulletRange,
        bulletSpeed,
        bulletDamage
    }

    public float[] bulletInfo = new float[3];

    protected GameProgressManager gameProgressManager;

    private void Awake()
    {
        gameProgressManager = FindAnyObjectByType<GameProgressManager>();
        AwakePlus();
    }

    protected virtual void AwakePlus()
    {

    }

    protected virtual void loadBulletInfo()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(ActiveTime(bulletInfo[(int)BulletInfoEnum.bulletRange] / bulletInfo[(int)BulletInfoEnum.bulletSpeed]));
    }


    // Update is called once per frame
    void Update()
    {
        UpdatePlus();
        
    }

    protected virtual void UpdatePlus()
    {
        transform.Translate(0, 0, bulletInfo[(int)BulletInfoEnum.bulletSpeed] * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ActiveTime());
    }

    protected virtual IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return null;
    }

}
