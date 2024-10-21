using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test06_Player : TestObject
{
    public GameObject player;

    Animator playerAnimator;

    int animatorAction;

    Ybot[] ybots;

    public GameObject Item1;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void onClickButton1(InputAction.CallbackContext context)
    {
        ybots = player.GetComponentsInChildren<Ybot>(false);

        //Debug.Log(ybots.Length);

        if (ybots.Length != 9)
            YBotPool.instance.SetActiveObject(new Vector3(0, 0.69f, 0));
    }

    protected override void onClickButton2(InputAction.CallbackContext obj)
    {
        ybots = player.GetComponentsInChildren<Ybot>(false);

        Debug.Log(ybots.Length);

        if (ybots.Length != 0)
        {
            YBotPool.instance.ObjDisable(ybots[Random.Range(0, ybots.Length)].gameObject);
        }

    }

    protected override void onClickButton3(InputAction.CallbackContext obj)
    {
        BarrelPool.instance.SetActiveObject(new Vector3(0, 0.5f, 4));

    }

    protected override void onClickButton4(InputAction.CallbackContext obj)
    {
        Instantiate(Item1, new Vector3(0, 1.5f, 5), Quaternion.identity);
    }
}
