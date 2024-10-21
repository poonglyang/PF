using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAriRaidPool : ObjectPool
{
    public static ObjectPool instance;
    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
}
