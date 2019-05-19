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
    private float timer = 0;
    public float leaderboardTime = 3f;
    private bool showLeaderboard = false;

    internal void Fade(float v)
    {
        var sr = playerUI.GetComponentsInChildren<Image>();
        foreach (Image sprite in sr)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, v);
        }

    }

    // Update is called once per frame
    void Update () {
        health1.fillAmount = (float)p1.health / (float)p1.maxHealth;
        health2.fillAmount = (float)p2.health / (float)p2.maxHealth;
        stamina1.fillAmount = (float)p1.GetComponent<MovementBehavior>().flyStamina / (float)p1.GetComponent<MovementBehavior>().flyMaxStamina;
        stamina2.fillAmount = (float)p2.GetComponent<MovementBehavior>().flyStamina / (float)p2.GetComponent<MovementBehavior>().flyMaxStamina;
        if(showLeaderboard)
        {
            if(timer < leaderboardTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                leaderboard.gameObject.SetActive(false);
                timer = 0;
                showLeaderboard = false;
                OnLeaderboardShown(this);
            }
        }
    }
    public void ShowLeaderboard(int firstPlayerScore, int secondPlayerScore)
    {
        leaderboard.gameObject.SetActive(true);
        leaderboard.ShowLeaderboard(firstPlayerScore, secondPlayerScore);
        showLeaderboard = true;
    }

}
