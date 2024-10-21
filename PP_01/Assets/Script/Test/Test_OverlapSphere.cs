using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_OverlapSphere : TestObject
{

    protected override void onClickButton1(InputAction.CallbackContext context)
    {
        XbotPool.instance.SetActiveObject(transform.position);
    }

    protected override void onClickButton2(InputAction.CallbackContext obj)
    {
        BarrelPool.instance.SetActiveObject(transform.position);
    }

    protected override void onClickButton3(InputAction.CallbackContext obj)
    {
        PannalPool.instance.SetActiveObject(new Vector3(transform.position.x, 0, transform.position.z));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
