using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILeaderboard : MonoBehaviour {

    public Text objective;
    public Text player1Text;
    public Text player2Text;
    public Text wizard1Text;
    public Text wizard2Text;
    public Image player1;
    public Image player2;

    public LeaderboardCookie[] cookieListP1;
    public LeaderboardCookie[] cookieListP2;

    private bool scoresSeted = false;

    public Animator anim;
    public float leaderboardTime = 3;
    private float timer = 0;
    public bool onScreen = false;

    public void Initialize()
    {
        player1.sprite = GameManager.Instance.GetPlayerById(1).wizardData.artwork;
        player2.sprite = GameManager.Instance.GetPlayerById(2).wizardData.artwork;
        player1Text.color = GameManager.Instance.GetPlayerById(1).playerColor;
        player2Text.color = GameManager.Instance.GetPlayerById(2).playerColor;
        wizard1Text.text = GameManager.Instance.GetPlayerById(1).wizardData.wizardName;
        wizard2Text.text = GameManager.Instance.GetPlayerById(2).wizardData.wizardName;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("Appear");
        onScreen = true;
    }

    private void Update()
    {
        if (onScreen)
        {
            timer += Time.deltaTime;
            if (timer >= leaderboardTime)
            {
                timer = 0;
                gameObject.SetActive(false);
                GameManager.Instance.CheckEndRound();
            }
        }
    }

    public void SetScores()
    {
        if (!scoresSeted)
        {
            scoresSeted = true;
            int p1WinCount = GameManager.Instance.GetPlayerById(1).winRounds;
            int p2WinCount = GameManager.Instance.GetPlayerById(2).winRounds;
            if (p1WinCount != 0)
                cookieListP1[p1WinCount - 1].ChangeToCookie();
            if (p2WinCount != 0)
                cookieListP2[p2WinCount - 1].ChangeToCookie();
        }
    }

    public void ResetScores()
    {
        for (int i = 0; i < cookieListP1.Length; i++)
        {
            cookieListP1[i].ChangeToEmpty();
            cookieListP2[i].ChangeToEmpty();
        }
    }

    private void OnDisable()
    {
        scoresSeted = false;
        onScreen = false;
    }

}
