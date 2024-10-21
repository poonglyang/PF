using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test10_AllDeath : TestObject
{
    PlayerManager player;
    Ybot[] ybots;

    GameProgressManager gameProgressManager;

    public GameObject test;
    public GameObject test2;
    public GameObject test3;

    Skill skillPanel;

    protected override void Awake()
    {
        base.Awake();

        player = FindAnyObjectByType<PlayerManager>();
        gameProgressManager = FindAnyObjectByType<GameProgressManager>();

        skillPanel = FindAnyObjectByType<Skill>();
    }

    protected override void onClickButton1(InputAction.CallbackContext context)
    {
        ybots = player.GetComponentsInChildren<Ybot>(false);

        if (ybots.Length != 9)
            YBotPool.instance.SetActiveObject(new Vector3(0, 0.69f, 0));
    }

    protected override void onClickButton2(InputAction.CallbackContext obj)
    {
        float[] bulletValue = gameProgressManager.PistolBulletValue;
        bulletValue[0] = 60;
        bulletValue[1] = 30;
        gameProgressManager.PistolBulletValue = bulletValue;
    }

    protected override void onClickButton3(InputAction.CallbackContext obj)
    {
        Instantiate(test, new Vector3(1, 0.5f, 10), Quaternion.identity);
        Instantiate(test3, new Vector3(-1, 0.5f, 10), Quaternion.identity);
    }

    protected override void onClickButton4(InputAction.CallbackContext obj)
    {
        //Instantiate(test2, new Vector3(0, 0.5f, 10), Quaternion.identity);
        skillPanel.ResorceIncrease(0.1f);
    }

    protected override void onClickButton5(InputAction.CallbackContext obj)
    {
        for (int i = 0; i < player.transform.childCount; i++)
        {
            YBotPool.instance.ObjDisable(player.transform.GetChild(i).gameObject);
        }
    }

    protected override void onClickButton6(InputAction.CallbackContext obj)
    {
        float[] bulletValue = gameProgressManager.FireRateValue;
        bulletValue[0] = 0.2f;
        
        gameProgressManager.FireRateValue = bulletValue;
    }

}
