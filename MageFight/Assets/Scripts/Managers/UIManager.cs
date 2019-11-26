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

    public GameObject playerUI;
    public UILeaderboard leaderboard;
    public GameObject PostGameUI;
    public Text playerLabel;
    public GameObject PlayerPresentationCanvas;
    public GameObject spellSelectionPanel;
    public GameObject rematchButton;
    public GameObject pauseMenuUI;
    public GameObject firstSelectedButtonPauseMenu;
    public BeginMatch beginMatchPanel;

    public void Initialize()
    {
        leaderboard.Initialize();
    }

    public void StartCountdown()
    {
        beginMatchPanel.gameObject.SetActive(true);
        beginMatchPanel.BeginCountdown();
        GameManager.Instance.currentMap.GenerateLevel();
    }

    public void ShowLeaderboard()
    {
        leaderboard.Show();
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
