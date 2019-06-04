﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager instance;
    public static UIManager Get() { return instance; }
    private void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
    public delegate void UIManagerActions(UIManager manager);
    public UIManagerActions OnLeaderboardShown;
    public PlayerBehavior p1;
    public PlayerBehavior p2;
    public Image health1;
    public Image health2;
    public Image stamina1;
    public Image stamina2;
    public GameObject playerUI;
    public UILeaderboard leaderboard;
    public GameObject PostGameUI;
    public Sprite[] playerLabelSprites;
    public Image playerLabel;
    private float timer = 0;
    public float leaderboardTime = 3f;
    private bool showLeaderboard = false;
    private bool leaderboardActive = false;

    public void Fade(float amount)
    {
        var sprites = playerUI.GetComponentsInChildren<Image>();
        foreach (Image image in sprites)
        {
            Color actualColor = image.color;
            actualColor.a = amount;
            image.color = actualColor;
        }
    }

    // Update is called once per frame
    void Update () {
        if (p1)
        {
            health1.fillAmount = (float)p1.health / (float)p1.maxHealth;
            stamina1.fillAmount = (float)p1.GetComponent<MovementBehavior>().flyStamina / (float)p1.GetComponent<MovementBehavior>().flyMaxStamina;
        }
        if (p2)
        {
            health2.fillAmount = (float)p2.health / (float)p2.maxHealth;
            stamina2.fillAmount = (float)p2.GetComponent<MovementBehavior>().flyStamina / (float)p2.GetComponent<MovementBehavior>().flyMaxStamina;
        }


        if (showLeaderboard)
        {
            if (!leaderboardActive)
            {
                if (timer < leaderboardTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    leaderboard.gameObject.SetActive(true);
                    leaderboardActive = true;
                    timer = 0;
                }
            }
            else
            {
                if (timer < leaderboardTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    leaderboard.gameObject.SetActive(false);
                    timer = 0;
                    showLeaderboard = false;
                    OnLeaderboardShown(this);
                    leaderboardActive = false;
                }
            }
        }
    }
    public void ShowLeaderboard(int firstPlayerScore, int secondPlayerScore)
    {
        leaderboard.ShowLeaderboard(firstPlayerScore, secondPlayerScore);
        showLeaderboard = true;
        leaderboardActive = false;
    }
    public void ShowPostGame(int winnerName)
    {
        int num = 0;
        if(winnerName > 0 && winnerName-1 < playerLabelSprites.Length){
            num = winnerName -1;
        }
        PostGameUI.SetActive(true);
        playerLabel.sprite = playerLabelSprites[num];
    }
    public void RematchPressed()
    {
        PostGameUI.SetActive(false);
        GameManager.Instance.EndGame();
    }
}
