using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPool : ObjectPool
{
    public static ObjectPool instance;

    protected override void Awake()
    {
        instance = this;
        base.Awake();
    }
    
}
