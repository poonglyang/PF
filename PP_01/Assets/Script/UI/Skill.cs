using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    Image skillResorceAmount;

    Transform[] canUses = new Transform[3];

    PlayerInput playerInputActions;

    PlayerManager player;

    Vector3 skill2EnablePos = new Vector3(-1.9f, 0, 7.5f);

    Vector3 skill3EnablePos = new Vector3(0, 10, 0);

    private void Awake()
    {
        skillResorceAmount = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        skillResorceAmount.fillAmount = 0;

        //Button[] button = GetComponentsInChildren<Button>();

        // 빌어먹게도 GetComponentsInChildren이 안먹는다

        for (int i = 0; i < 3; i++)
        {
            canUses[i] = transform.GetChild(2).GetChild(i).GetChild(2);
        }

        playerInputActions = new PlayerInput();

        player = FindAnyObjectByType<PlayerManager>();

    }

    private void OnEnable()
    {
        playerInputActions.Kema.Enable();

        playerInputActions.Kema.Skill1.performed += Skill1;
        playerInputActions.Kema.Skill2.performed += Skill2;
        playerInputActions.Kema.Skill3.performed += Skill3;

    }

    private void OnDisable()
    {
        playerInputActions.Kema.Skill1.performed -= Skill1;
        playerInputActions.Kema.Skill2.performed -= Skill2;
        playerInputActions.Kema.Skill3.performed -= Skill3;
        playerInputActions.Kema.Disable();
    }

    private void Skill1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (skillResorceAmount.fillAmount > 0.3f)
        {
            skillResorceAmount.fillAmount -= 0.3f;
            //player.Ybotinv(2f);
            SkillBarrierPool.instance.SetActiveObject(player.transform.position + Vector3.forward + Vector3.up);
            SkillPannelRecheck();
        }
    }

    private void Skill2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (skillResorceAmount.fillAmount > 0.6f)
        {
            skillResorceAmount.fillAmount -= 0.6f;
            SkillMissilePool.instance.SetActiveObject(skill2EnablePos);
            Debug.Log($"SkillPannelRecheck의 skillResorceGuage {skillResorceAmount.fillAmount}");
            SkillPannelRecheck();
        }
    }

    private void Skill3(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (skillResorceAmount.fillAmount > 0.99f)
        {
            skillResorceAmount.fillAmount -= 0.99f;
            //Instantiate(skill3);
            SkillAriRaidPool.instance.SetActiveObject(skill3EnablePos);
            SkillPannelRecheck();
        }
    }

    public void ResorceIncrease(float increaseResoreValue)
    {
        skillResorceAmount.fillAmount += increaseResoreValue;

        SkillPannelRecheck();
    }

    void SkillPannelRecheck()
    {
        float skillResorceGuage = skillResorceAmount.fillAmount;

        

        if(0.99f < skillResorceGuage)
        {
            canUses[0].gameObject.SetActive(false);
            canUses[1].gameObject.SetActive(false);
            canUses[2].gameObject.SetActive(false);
        }
        else if(0.6f < skillResorceGuage)
        {
            canUses[0].gameObject.SetActive(true);
            canUses[1].gameObject.SetActive(false);
            canUses[2].gameObject.SetActive(false);
        }
        else if (0.3f < skillResorceGuage)
        {
            canUses[0].gameObject.SetActive(true);
            canUses[1].gameObject.SetActive(true);
            canUses[2].gameObject.SetActive(false);
        }
        else
        {
            canUses[0].gameObject.SetActive(true);
            canUses[1].gameObject.SetActive(true);
            canUses[2].gameObject.SetActive(true);
        }

    }
}
