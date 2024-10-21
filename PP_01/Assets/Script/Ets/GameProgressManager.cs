using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    public enum BulletObjcet
    {
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


    // -------------------- 권총 정보 초기 설정 -------------------------------------

    /// <summary>
    /// 권총 정보 가져옴
    /// </summary>
    public float[] pistolBulletValue = new float[4] {
        GameManager.instance.GetBulletInfo((int)BulletObjcet.pistolRange),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.pistolBulletSpeed),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.pistolDamage),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.pistolFireRate)
    };

    /// <summary>
    /// 권총 정보 변경시 자동 변경을 위한 프로퍼티
    /// 개별적으로 값을 바꿨을 경우 set 발동 안함
    /// </summary>
    public float[] PistolBulletValue
    {
        get => pistolBulletValue;
        set
        {
            pistolBulletValue = value;
            psitolBulletInfoChange?.Invoke();
            Debug.Log("권총 총알 스팩 변함");
        }
    }

    /// <summary>
    /// 권총 정보가 변경되었다는 델리게이트
    /// </summary>
    public Action psitolBulletInfoChange;

    // ------------------------------------------------------------------------------


    // -------------------- 샷건 정보 초기 설정 -------------------------------------

    /// <summary>
    /// 샷건 정보 가져옴
    /// </summary>
    public float[] shotgunBulletValue = new float[4] {
        GameManager.instance.GetBulletInfo((int)BulletObjcet.shotgunRange),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.shotgunBulletSpeed),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.shotgunDamage),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.shotgunRange)
    };

    /// <summary>
    /// 샷건 정보 변경시 자동 변경을 위한 프로퍼티
    /// 개별적으로 값을 바꿨을 경우 set 발동 안함
    /// </summary>
    public float[] ShotgunBulletValue
    {
        get => shotgunBulletValue;
        set
        {
            shotgunBulletValue = value;
            sniperBulletInfoChange?.Invoke();
            Debug.Log("샷건 총알 스팩 변함");
        }
    }

    /// <summary>
    /// 샷건 정보가 변경되었다는 델리게이트
    /// </summary>
    public Action shotgunBulletInfoChange;

    // ------------------------------------------------------------------------------

    // -------------------- AR 정보 초기 설정 -------------------------------------
    /// <summary>
    /// AR 정보 가져옴
    /// </summary>
    public float[] arBulletValue = new float[4] {
        GameManager.instance.GetBulletInfo((int)BulletObjcet.ARRange),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.ARBulletSpeed),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.ARDamage),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.ARFireRate)
    };

    /// <summary>
    /// AR 변경시 자동 변경을 위한 프로퍼티
    /// 개별적으로 값을 바꿨을 경우 set 발동 안함
    /// </summary>
    public float[] ARBulletValue
    {
        get => arBulletValue;
        set
        {
            arBulletValue = value;
            arBulletInfoChange?.Invoke();
            Debug.Log("스나이퍼 총알 Range 스팩 변함");
        }
    }

    /// <summary>
    /// AR 정보가 변경되었다는 델리게이트
    /// </summary>
    public Action arBulletInfoChange;

    // ------------------------------------------------------------------------------

    // -------------------- SR 정보 초기 설정 -------------------------------------
    /// <summary>
    /// 정보 가져옴
    /// </summary>
    public float[] sniperBulletValue = new float[4] {
        GameManager.instance.GetBulletInfo((int)BulletObjcet.SRRange),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.SRBulletSpeed),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.SRDamage),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.SRFireRate)
    };

    /// <summary>
    /// SR 변경시 자동 변경을 위한 프로퍼티
    /// 개별적으로 값을 바꿨을 경우 set 발동 안함
    /// </summary>
    public float[] SniperBulletValue
    {
        get => sniperBulletValue;
        set
        {
            sniperBulletValue = value;
            sniperBulletInfoChange?.Invoke();
            Debug.Log("스나이퍼 총알 Range 스팩 변함");
        }
    }

    /// <summary>
    /// SR 정보가 변경되었다는 델리게이트
    /// </summary>
    public Action sniperBulletInfoChange;

    // ------------------------------------------------------------------------------

    public float[] fireRateValue = new float[4]
    {
        GameManager.instance.GetBulletInfo((int)BulletObjcet.pistolFireRate),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.shotgunFireRate),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.ARFireRate),
        GameManager.instance.GetBulletInfo((int)BulletObjcet.SRFireRate)
    };

    public float[] FireRateValue
    {
        get => fireRateValue;
        set
        {
            fireRateValue = value;
            fireRateValueInfoChange?.Invoke();
            Debug.Log("특정 총알의 연사속도 변경");
        }
    }

    public Action fireRateValueInfoChange;

    // ------------------------------------------------------------------------------

    

    private void Awake()
    {
        instance = this;
        fireRateValue = new float[4]
        {
            GameManager.instance.GetBulletInfo((int)BulletObjcet.pistolFireRate),
            GameManager.instance.GetBulletInfo((int)BulletObjcet.shotgunFireRate),
            GameManager.instance.GetBulletInfo((int)BulletObjcet.ARFireRate),
            GameManager.instance.GetBulletInfo((int)BulletObjcet.SRFireRate)
        };
    }

    

}
