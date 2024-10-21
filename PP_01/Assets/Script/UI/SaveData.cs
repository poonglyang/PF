using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// json 유틸리티에서 사용하기 위해서는 반드시 직렬화가 가능한 클래스이여야 한다.
[Serializable] // 이 아래 클래스는 직렬화되는 클래스이다.
public class SaveData
{
    public int coinData;
    public int cashData;
    public int[] shopObjectStepData;


}
