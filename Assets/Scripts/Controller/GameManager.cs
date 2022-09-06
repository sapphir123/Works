using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGetFront = false;

    public Image image;
    public Text reGame;
    public Text hpText;
    public Button exit;

    private int countEnemy = 0;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        EventCenter.AddListener<TargetEventArgs>(EventType.Event_PlayerHpDown, ShowHp);
        EventCenter.AddListener(EventType.Event_PlayerDeath, PlayerDeath);
        EventCenter.AddListener(EventType.Event_EnemyDeath, EneemyDeath);
        image.fillAmount = 1f;

        reGame.gameObject.SetActive(false);

        exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Update()
    {
        image.transform.Rotate(new Vector3(0, 0, 0.5f));
        if (!isGetFront && Input.GetKeyDown(KeyCode.Space))
        {
            ArrayFrontManager.Instance.CreatArayFront();
            isGetFront = true;
        }

        if (isGetFront && Input.GetKeyDown(KeyCode.F))
            isGetFront = false;
    }

    public void ShowHp(TargetEventArgs arg)
    {
        float one = (255 + 255) / 60;//（255+255）除以最大取值的三分之二
        if (arg.hp < 0)
            arg.hp = 0;
        float value = 90 - 90 * arg.hp / 1000;
        hpText.text = "血量" + arg.hp;
        //Debug.Log(value);
        int r = 0, g = 0, b = 0;
        if (value < 30)//第一个三等分
        {
            r = (int)(one * value);
            g = 255;
        }
        else if (value >= 30 && value < 60)//第二个三等分
        {
            r = 255;
            g = 255 - (int)((value - 30) * one);//val减最大取值的三分之一
        }
        else { r = 255; }//最后一个三等分

        image.color = new Color(r, g, b);
        hpText.color = new Color(r, g, b);
    }

    public void PlayerDeath()
    {
        reGame.gameObject.SetActive(true);
        reGame.text = "您已经生存了" + Time.time + "s" + "\n" + "您已经击杀了" + countEnemy + "个敌人";
        Time.timeScale = 0;
    }

    public void EneemyDeath()
    {
        countEnemy++;
    }
}

public class TargetEventArgs : EventArgs
{
    public float hp;
    public float time;
}
