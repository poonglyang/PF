using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodsUIBase : MonoBehaviour
{
    TextMeshProUGUI goodsText;

    /// <summary>
    /// 초기값은 항상 0
    /// </summary>
    public int goods = 0;

    public int Goods
    {
        get => goods;
        set
        {
            goods = value;

            if(goodsText != null)
            {
                goodsText = GetComponentInChildren<TextMeshProUGUI>();
                goodsText.text = goods.ToString();
            }
        }
    }

    private void Awake()
    {
        goodsText = GetComponentInChildren<TextMeshProUGUI>();
        goodsText.text = goods.ToString();
    }

    
}
