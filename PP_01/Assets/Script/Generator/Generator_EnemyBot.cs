using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator_EnemyBot : Generator_Base
{
    

    private void Awake()
    {
        StartCoroutine(SpawnXbot());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnXbot()
    {
        while (true)
        {
            float spawnX = Random.Range(-3.5f, 3.5f);

            yield return new WaitForSeconds(spawnTime);
            XbotPool.instance.SetActiveObject(new Vector3(spawnX, 0, transform.position.z));
            XbotPool.instance.SetActiveObject(new Vector3(spawnX, 0, transform.position.z));
        }
    }
}
