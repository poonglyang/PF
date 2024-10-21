using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test09_AddMoney : TestObject
{
    Skill skillPanel;

    public GameObject player;

    Ybot[] ybots;

    CoinUI coinUI;

    CashUI cashUI;

    GameProgressManager gameProgressManager;
    protected override void Awake()
    {
        base.Awake();
        skillPanel = FindAnyObjectByType<Skill>();

        coinUI = FindAnyObjectByType<CoinUI>();

        cashUI = FindAnyObjectByType<CashUI>();

        gameProgressManager = FindAnyObjectByType<GameProgressManager>();
    }

    protected override void onClickButton1(InputAction.CallbackContext context)
    {
        skillPanel.ResorceIncrease(0.1f);
    }

    protected override void onClickButton2(InputAction.CallbackContext obj)
    {
        ybots = player.GetComponentsInChildren<Ybot>(false);

        if (ybots.Length != 9)
            YBotPool.instance.SetActiveObject(new Vector3(0, 0.69f, 0));
    }

    protected override void onClickButton3(InputAction.CallbackContext obj)
    {
        //CoinPool.instance.SetActiveObject(new Vector3(0, 1, 20));
        CoinPool.instance.SetActiveObject(player.transform.position);
    }

    protected override void onClickButton4(InputAction.CallbackContext obj)
    {
        //CashPool.instance.SetActiveObject(player.transform.position);
        float[] bulletValue = gameProgressManager.PistolBulletValue;
        bulletValue[0] = 60;
        bulletValue[1] = 30;
        gameProgressManager.PistolBulletValue = bulletValue;
    }

    protected override void onClickButton5(InputAction.CallbackContext obj)
    {
        for (int i = 0; i < player.transform.childCount; i++)
        {
            YBotPool.instance.ObjDisable(player.transform.GetChild(i).gameObject);
        }
    }
}
