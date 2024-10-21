using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GunInfo : MonoBehaviour
{
    TextMeshProUGUI[] gunInfo = new TextMeshProUGUI[16];

    Shop shop;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j ++)
            {
                gunInfo[i*4 + j] = transform.GetChild(i).GetChild(j).GetChild(3).GetComponent<TextMeshProUGUI>();
            }
        }

        //gunInfo = GetComponentsInChildren<TextMeshProUGUI>();
        shop = transform.parent.GetComponent<Shop>();
        shop.somethingBuy += () =>
        {
            RefreshGunInfo();
        };

        RefreshGunInfo();
    }

    void RefreshGunInfo()
    {
        for(int i = 0; i < gunInfo.Length; i ++)
        {
            Debug.Log(i);
            gunInfo[i].text = GameManager.instance.GetBulletInfo(i).ToString();
        }
    }
}
