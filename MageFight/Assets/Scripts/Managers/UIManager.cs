using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public GameObject playerUI;
    public UILeaderboard leaderboard;
    public GameObject PostGameUI;
    public Text playerLabel;
    private float timer = 0;
    public float leaderboardTime = 3f;
    private bool showLeaderboard = false;
    private bool leaderboardActive = false;
    public GameObject PlayerPresentationCanvas;
    public GameObject spellSelectionPanel;
    public GameObject rematchButton;
    public GameObject pauseMenuUI;
    public GameObject firstSelectedButtonPauseMenu;
    public BeginMatch beginMatchPanel;

    private void Start()
    {
        leaderboard.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
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
                    leaderboard.SetScores();
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

    public void StartCountdown()
    {
        beginMatchPanel.gameObject.SetActive(true);
        beginMatchPanel.BeginCountdown();
        GameManager.Instance.currentMap.GenerateLevel();
    }

    public void ShowLeaderboard()
    {
        showLeaderboard = true;
        leaderboardActive = false;
    }

    public void ShowPostGame(int winnerName)
    {
        EventSystem evt = EventSystem.current;
        evt.SetSelectedGameObject(rematchButton);
        PostGameUI.SetActive(true);
        playerLabel.text = "PLAYER " + winnerName.ToString();
    }

    public void RematchPressed()
    {
        leaderboard.ResetScores();
        PostGameUI.SetActive(false);
        GameplayManager.Get().SendEvent(GameplayManager.Events.RematchSelected);
    }

    public void SetPlayerPresentationUI(bool value)
    {
        if(value)
        {
            PlayerPresentationCanvas.SetActive(true);
            PlayerPresentationCanvas.GetComponent<Animator>().SetTrigger("Start");
        }
        else
        {
            PlayerPresentationCanvas.SetActive(false);
            spellSelectionPanel.SetActive(true);
        }
    }
    public void SetPauseMenuUI(bool value)
    {
        if(value)
        {
            pauseMenuUI.SetActive(true);
            EventSystem evt = EventSystem.current;
            evt.SetSelectedGameObject(firstSelectedButtonPauseMenu);
        }
        else
            pauseMenuUI.SetActive(false);
    }

    public void ResetUI()
    {
        var playersUI = playerUI.GetComponentsInChildren<PlayerUI>();
        foreach (PlayerUI pUI in playersUI)
        {
            pUI.ResetUICookies();
        }
    }
}
