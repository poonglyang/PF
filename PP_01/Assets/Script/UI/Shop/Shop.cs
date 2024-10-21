using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Shop : MonoBehaviour
{
    Animator animator;

    readonly int shopActive = Animator.StringToHash("ShopActive");

    /// <summary>
    /// 뷰포인트 Context의 Transform
    /// </summary>
    Transform viewPointContext;

    TextMeshProUGUI pistolRangeStep;
    TextMeshProUGUI pistolRangePrice;

    TextMeshProUGUI[] shopObjectStep;
    TextMeshProUGUI[] shopObjectPrice;

    enum ShopObjcet {
        pistolRange,
        pistolBulletSpeed,
        pistolDamage,
        pistolFireRate,
        shotgunRange,
        shotgunBulletSpeed,
        shotgunDamage,
        shotgunFireRate,
        ARRange,
        ARBulletSpeed,
        ARDamage,
        ARFireRate,
        SRRange,
        SRBulletSpeed,
        SRDamage,
        SRFireRate
    }

    /// <summary>
    /// 무언가를 샀을 때 GunInfo를 바꿔주기 위한 델리게이트
    /// </summary>
    public Action somethingBuy;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        viewPointContext = transform.GetChild(2).GetChild(0).GetChild(0);

        int contextChildCount = viewPointContext.childCount;

        Transform step;

        Transform price;

        shopObjectStep = new TextMeshProUGUI[contextChildCount];
        shopObjectPrice = new TextMeshProUGUI[contextChildCount];

        for (int i =0; i < contextChildCount; i ++)
        {
            step = viewPointContext.GetChild(i).GetChild(2);
            shopObjectStep[i] = step.GetComponent<TextMeshProUGUI>();
            price = viewPointContext.GetChild(i).GetChild(4).GetChild(0);
            shopObjectPrice[i] = price.GetComponent<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        for (int i = 0; i < shopObjectStep.Length; i++)
        {
            ReplaceStepAndPrice(i);
        }
    }

    public void ShopClose()
    {
        animator.SetBool(shopActive, false);
    }

    void ReplaceStepAndPrice(int shopObject)
    {

        if(GameManager.instance.ShopObjectStep[shopObject] == 3)
        {
            shopObjectStep[shopObject].text = "MAX";
            shopObjectPrice[shopObject].transform.parent.gameObject.SetActive(false);
        }
        else
        {
            shopObjectStep[shopObject].text = GameManager.instance.ShopObjectStep[shopObject].ToString();
            //shopObjectPrice[shopObject].text = GameManager.instance.ShopObjectPrice[shopObject, GameManager.instance.ShopObjectStep[shopObject]].ToString();
            shopObjectPrice[shopObject].text = GameManager.instance.ShopObjectPrice2[3 * shopObject + GameManager.instance.ShopObjectStep[shopObject]].ToString();


        }
    }

    private void Buy(int objectType)
    {
        if (GameManager.instance.ShopObjectStep[objectType] < 3 && GameManager.instance.Coin > GameManager.instance.ShopObjectPrice2[GameManager.instance.ShopObjectStep[objectType]] - 1)
        {
            GameManager.instance.Coin -= GameManager.instance.ShopObjectPrice2[GameManager.instance.ShopObjectStep[objectType]];
            GameManager.instance.ShopObjectStep[objectType]++;
            somethingBuy?.Invoke();
        }
        else
        {
            Debug.Log("돈이 없거나 MAX업그레이드임");
        }
        ReplaceStepAndPrice(objectType);
    }

    public void PistolRangeBuy()
    {
        int enumValue = (int)ShopObjcet.pistolRange;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void PistolBulletSpeedBuy()
    {
        int enumValue = (int)ShopObjcet.pistolBulletSpeed;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void PistolDamageBuy()
    {
        int enumValue = (int)ShopObjcet.pistolDamage;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void PistolFireRateBuy()
    {
        int enumValue = (int)ShopObjcet.pistolFireRate;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }
    public void ShotgunRangeBuy()
    {
        int enumValue = (int)ShopObjcet.shotgunRange;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }
    public void ShotgunBulletSpeedBuy()
    {
        int enumValue = (int)ShopObjcet.shotgunBulletSpeed;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void ShotgunDamageBuy()
    {
        int enumValue = (int)ShopObjcet.shotgunDamage;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void ShotgunFireRateBuy()
    {
        int enumValue = (int)ShopObjcet.shotgunFireRate;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }
    public void ARRangeBuy()
    {
        int enumValue = (int)ShopObjcet.ARRange;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void ARBulletSpeedBuy()
    {
        int enumValue = (int)ShopObjcet.ARBulletSpeed;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void ARDamageBuy()
    {
        int enumValue = (int)ShopObjcet.ARDamage;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void ARFireRateBuy()
    {
        int enumValue = (int)ShopObjcet.ARFireRate;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }
    public void SRRangeBuy()
    {
        int enumValue = (int)ShopObjcet.SRRange;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void SRBulletSpeedBuy()
    {
        int enumValue = (int)ShopObjcet.SRBulletSpeed;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void SRDamageBuy()
    {
        int enumValue = (int)ShopObjcet.SRDamage;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

    public void SRFireRateBuy()
    {
        int enumValue = (int)ShopObjcet.SRFireRate;
        Buy(enumValue);
        GameManager.instance.GetBulletInfo(enumValue);
    }

}
