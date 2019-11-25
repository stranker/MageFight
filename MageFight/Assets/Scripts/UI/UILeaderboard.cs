using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILeaderboard : MonoBehaviour {

    public Text objective;
    public Text player1Text;
    public Text player2Text;
    public Image player1;
    public Image player2;
    public Transform cookiePanelP1;
    public Transform cookiePanelP2;
    public LeaderboardCookie leaderCookie;
    public List<LeaderboardCookie> cookieListP1 = new List<LeaderboardCookie>();
    public List<LeaderboardCookie> cookieListP2 = new List<LeaderboardCookie>();
    private bool scoresSeted = false;
    public Animator anim;
    public float leaderboardTime = 3;
    private float timer = 0;
    public bool onScreen = false;


    private void Start()
    {
        objective.text = "FIRST TO " + GameManager.Instance.roundsToWin.ToString() + " COOKIES";
        for (int i = 0; i < GameManager.Instance.roundsToWin; i++)
        {
            CreatePlayerCookies(cookiePanelP1, cookieListP1);
            CreatePlayerCookies(cookiePanelP2, cookieListP2);
        }
        player1.sprite = GameManager.Instance.GetPlayerById(1).wizardData.artwork;
        player2.sprite = GameManager.Instance.GetPlayerById(2).wizardData.artwork;
        player1Text.color = GameManager.Instance.GetPlayerById(1).playerColor;
        player2Text.color = GameManager.Instance.GetPlayerById(2).playerColor;
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

    private void CreatePlayerCookies(Transform cookiePanel, List<LeaderboardCookie> cookieList)
    {
        GameObject cookie = Instantiate(leaderCookie.gameObject, cookiePanel);
        cookieList.Add(cookie.GetComponent<LeaderboardCookie>());
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
        for (int i = 0; i < cookieListP1.Count; i++)
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
