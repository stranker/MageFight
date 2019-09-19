using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILeaderboard : MonoBehaviour {

    public Text objective;
    public Transform cookiePanelP1;
    public Transform cookiePanelP2;
    public LeaderboardCookie leaderCookie;
    public List<LeaderboardCookie> cookieListP1 = new List<LeaderboardCookie>();
    public List<LeaderboardCookie> cookieListP2 = new List<LeaderboardCookie>();

    private void Start()
    {
        objective.text = "FIRST TO " + GameManager.Instance.roundsToWin.ToString() + " COOKIES";
        for (int i = 0; i < GameManager.Instance.roundsToWin; i++)
        {
            CreatePlayerCookies(cookiePanelP1, cookieListP1);
            CreatePlayerCookies(cookiePanelP2, cookieListP2);
        }
        gameObject.SetActive(false);
    }

    private void CreatePlayerCookies(Transform cookiePanel, List<LeaderboardCookie> cookieList)
    {
        GameObject cookie = Instantiate(leaderCookie.gameObject, cookiePanel);
        cookieList.Add(cookie.GetComponent<LeaderboardCookie>());
    }

    public void SetScores(int firstPlayerScore, int secondPlayerScore)
    {
        if (firstPlayerScore != 0)
            cookieListP1[firstPlayerScore - 1].ChangeToCookie();
        if (secondPlayerScore != 0)
            cookieListP2[secondPlayerScore - 1].ChangeToCookie();
    }
}
