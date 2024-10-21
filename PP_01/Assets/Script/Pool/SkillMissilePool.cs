using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMissilePool : ObjectPool
{
    public static ObjectPool instance;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
}
