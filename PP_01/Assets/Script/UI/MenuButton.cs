using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    Animator shopAnimator;

    readonly int shopActive = Animator.StringToHash("ShopActive");

    private void Awake()
    {
        shopAnimator = FindAnyObjectByType<Shop>().gameObject.GetComponent<Animator>();
    }

    public void GameStart()
    {
        //restart.onClick.AddListener(() => SceneManager.LoadScene(0));
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        GameManager.instance.SaveGunData();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(); // 어플리케이션 종료
        #endif
    }

    public void ShopOpen()
    {
        shopAnimator.SetBool(shopActive, true);
    }

    public void test3()
    {
        SceneManager.LoadScene(1);
    }
}
