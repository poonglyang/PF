using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObject : MonoBehaviour
{
    Test inputAction;
    protected virtual void Awake()
    {
        
        inputAction = new Test();
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.testButton.Button1.performed += onClickButton1;
        inputAction.testButton.Button2.performed += onClickButton2;
        inputAction.testButton.Button3.performed += onClickButton3;
        inputAction.testButton.Button4.performed += onClickButton4;
        inputAction.testButton.Button5.performed += onClickButton5;
        inputAction.testButton.Button6.performed += onClickButton6;

    }

    
    private void OnDisable()
    {
        inputAction.testButton.Button6.performed -= onClickButton6;
        inputAction.testButton.Button5.performed -= onClickButton5;
        inputAction.testButton.Button4.performed -= onClickButton4;
        inputAction.testButton.Button3.performed -= onClickButton3;
        inputAction.testButton.Button2.performed -= onClickButton2;
        inputAction.testButton.Button1.performed -= onClickButton1;
        inputAction.Disable();
    }

    protected virtual void onClickButton6(InputAction.CallbackContext obj)
    {
        
    }


    protected virtual void onClickButton5(InputAction.CallbackContext obj)
    {
    }

    protected virtual void onClickButton4(InputAction.CallbackContext obj)
    {
    }

    protected virtual void onClickButton3(InputAction.CallbackContext obj)
    {
        
    }

    protected virtual void onClickButton2(InputAction.CallbackContext obj)
    {
        
    }

    protected virtual void onClickButton1(InputAction.CallbackContext context)
    {
        XbotPool.instance.SetActiveObject(new Vector3(transform.position.x, transform.position.y, 50));
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
