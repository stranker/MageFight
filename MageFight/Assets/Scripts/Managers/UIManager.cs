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
    public GameObject PlayerPresentationCanvas;
    public GameObject spellSelectionPanel;
    public GameObject pauseMenuUI;
    public GameObject firstSelectedButtonPauseMenu;
    public BeginMatch beginMatchPanel;
    public PostGame postGame;
    public GameObject rematchButton;

    public void Initialize()
    {
        leaderboard.Initialize();
    }

    public void Rematch()
    {
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

    public void ShowPostGame(int winnerIdx)
    {
        postGame.SetWinner(winnerIdx);
        EventSystem evt = EventSystem.current;
        evt.SetSelectedGameObject(rematchButton);
    }

    public void SetPlayerPresentationUI(bool value)
    {
        if(value)
        {
            PlayerPresentationCanvas.SetActive(true);
            AkSoundEngine.PostEvent(AudioEvents.eventsIDs[AudioEvents.EventsKeys.Versus_Animation_Start.ToString()], this.gameObject);
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

    public void RematchPressed()
    {
        leaderboard.ResetScores();
        postGame.Hide();
        GameplayManager.Get().SendEvent(GameplayManager.Events.RematchSelected);
    }
}
