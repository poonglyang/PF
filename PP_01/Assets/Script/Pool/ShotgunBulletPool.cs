using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletPool : BulletObjectPool
{
    public static BulletObjectPool instance;

    protected override void Awake()
    {
        instance = this;
        base.Awake();
    }
}
