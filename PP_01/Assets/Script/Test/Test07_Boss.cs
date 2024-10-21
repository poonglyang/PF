using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test07_Boss : TestObject
{
    public GameObject bossSpawnEffect;
    Vector3 bossSpawnPos = new Vector3(0, 0, 16);

    Ybot[] ybots;

    public GameObject player;

    protected override void onClickButton1(InputAction.CallbackContext context)
    {
        StartCoroutine(BossSpawn());
    }

    protected override void onClickButton2(InputAction.CallbackContext obj)
    {
        ybots = player.GetComponentsInChildren<Ybot>(false);

        //Debug.Log(ybots.Length);

        if (ybots.Length != 9)
            YBotPool.instance.SetActiveObject(new Vector3(0, 0.69f, 0));
    }

    protected override void onClickButton3(InputAction.CallbackContext obj)
    {
        BossSpellEffectPool.instance.SetActiveObject(new Vector3(0, 1.5f, 16));
    }

    IEnumerator BossSpawn()
    {
        Instantiate(bossSpawnEffect, bossSpawnPos, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        BossPool.instance.SetActiveObject(bossSpawnPos);
    }
}
