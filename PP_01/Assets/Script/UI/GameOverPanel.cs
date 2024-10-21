using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverPanel : MonoBehaviour
{
    public static GameOverPanel instance;

    public int commonZombDieCount = 0;

    public int specialZombDieCount = 0;

    public int bossZombDieCount = 0;

    public int coinCount = 0;

    public int cashCount = 0;

    TextMeshProUGUI[] score = new TextMeshProUGUI[4];

    TextMeshProUGUI[] goods;

    PlayerManager player;

    Animator animator;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < 3; i ++)
        {
            score[i] = transform.GetChild(1).GetChild(2).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        score[3] = transform.GetChild(1).GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>();

        goods = transform.GetChild(1).GetChild(3).GetComponentsInChildren<TextMeshProUGUI>();

        player = FindAnyObjectByType<PlayerManager>();

        animator = GetComponent<Animator>();

        player.onDie += () =>
        {
            animator.SetBool("GameOver", true);
            Value();
        };
    }


    int Score()
    {
        return commonZombDieCount * 10 + specialZombDieCount * 50 + bossZombDieCount * 100;
    }

    void Value()
    {
        score[0].text = commonZombDieCount.ToString();
        score[1].text = specialZombDieCount.ToString();
        score[2].text = bossZombDieCount.ToString();
        score[3].text = Score().ToString();
        goods[0].text = cashCount.ToString();
        goods[1].text = coinCount.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void GotoTitleMenu()
    {
        SceneManager.LoadScene(0);
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
}
