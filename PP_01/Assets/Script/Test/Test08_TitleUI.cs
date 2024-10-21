using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test08_TitleUI : TestObject
{

    protected override void onClickButton1(InputAction.CallbackContext context)
    {
        GameManager.instance.Coin+= 30;
    }

    protected override void onClickButton2(InputAction.CallbackContext obj)
    {
        GameManager.instance.Cash++;
    }

    

}
