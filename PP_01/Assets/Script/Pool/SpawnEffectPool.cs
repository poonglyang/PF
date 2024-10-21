using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffectPool : ObjectPool
{
    public static ObjectPool instance;

    protected override void Awake()
    {
        instance = this;
        base.Awake();
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
