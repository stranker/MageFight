using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
    private static GameplayManager instance;
    public static GameplayManager Get() { return instance; }

    public enum States
    {
        GameStart,
        PlayerPresentation,
        SpellSelection,
        Countdown,
        Gameplay,
        DeathCamera,
        Leaderboard,
        PostGame,
        Rematch,
        Pause,
        ResumingGameplay,
        Count
    }
    public enum Events
    { 
        StartGame,
        PlayerPresentationEnd,
        SpellsSelected,
        CountdownEnd,
        PlayerDead,
        DeathCameraEnd,
        LeaderboardShownWinner,
        LeaderboardShownNoWinner,
        ExitSelected,
        RematchSelected,
        GoToSpellSelection,
        PauseGameplay,
        ResumeGameplay,
        Count
    }
    private FSM fsm;
    int lastState = -1;

    private void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);

        fsm = new FSM((int)States.Count, (int)Events.Count, (int)States.GameStart);

        fsm.SetRelation((int)States.GameStart, (int)Events.StartGame, (int)States.PlayerPresentation);
        fsm.SetRelation((int)States.PlayerPresentation, (int)Events.PlayerPresentationEnd, (int)States.SpellSelection);
        fsm.SetRelation((int)States.SpellSelection, (int)Events.SpellsSelected, (int)States.Countdown);
        fsm.SetRelation((int)States.Countdown, (int)Events.CountdownEnd, (int)States.Gameplay);
        fsm.SetRelation((int)States.Gameplay, (int)Events.PlayerDead, (int)States.DeathCamera);
        fsm.SetRelation((int)States.DeathCamera, (int)Events.DeathCameraEnd, (int)States.Leaderboard);
        fsm.SetRelation((int)States.Leaderboard, (int)Events.LeaderboardShownNoWinner, (int)States.SpellSelection);
        fsm.SetRelation((int)States.Leaderboard, (int)Events.LeaderboardShownWinner, (int)States.PostGame);
        fsm.SetRelation((int)States.PostGame, (int)Events.RematchSelected, (int)States.Rematch);
        fsm.SetRelation((int)States.Rematch, (int)Events.GoToSpellSelection, (int)States.SpellSelection);
        fsm.SetRelation((int)States.Gameplay, (int)Events.PauseGameplay, (int)States.Pause);
        fsm.SetRelation((int)States.Pause, (int)Events.PauseGameplay, (int)States.ResumingGameplay);
        fsm.SetRelation((int)States.ResumingGameplay, (int)Events.ResumeGameplay, (int)States.Gameplay);
        fsm.SetRelation((int)States.Pause, (int)Events.RematchSelected, (int)States.Rematch);
    }
    private void Update()
    {
        if (lastState != fsm.GetState())
        {
            lastState = fsm.GetState();
            print((States)lastState);
            switch(lastState)
            {
                case (int)States.PlayerPresentation:
                    PlayerPresentation();
                    break;
                case (int)States.SpellSelection:
                    SpellSelection();
                    break;
                case (int)States.Countdown:
                    Countdown();
                    break;
                case (int)States.Gameplay:
                    Gameplay();
                    break;
                case (int)States.DeathCamera:
                    DeathCamera();
                    break;
                case (int)States.Leaderboard:
                    Leaderboard();
                    break;
                case (int)States.PostGame:
                    PostGame();
                    break;
                case (int)States.Rematch:
                    Rematch();
                break;
                case (int)States.Pause:
                    Pause();
                    break;
                case (int)States.ResumingGameplay:
                    ResumingGameplay();
                    break;
            }
        }
    }


    public void PlayerPresentation()
    {
        CameraManager.Get().ActivateCamerasPlayerPresentation();
        UIManager.Get().SetPlayerPresentationUI(true);
    }
    public void SpellSelection()
    {
        UIManager.Get().SetPlayerPresentationUI(false);
        CameraManager.Get().ActivateCamerasGameplay();
    }
    public void Countdown()
    {
        UIManager.Get().StartCountdown();
    }
    public void Gameplay()
    {
        GameManager.Instance.StartRound();
    }
    public void DeathCamera()
    {
        CameraManager.Get().ActivateDeathcam();
    }
    public void Leaderboard()
    {
        GameManager.Instance.EndRound();
    }
    public void PostGame()
    {
    }
    private void Rematch()
    {
        GameManager.Instance.EndGame();
        UIManager.Get().SetPauseMenuUI(false);
        UIManager.Get().ResetUI();
        GameManager.Instance.InitializeRound();
        SendEvent(Events.GoToSpellSelection);
    }
    public void SendEvent(Events evt)
    {
        fsm.SendEvent((int)evt);
    }
    private void Pause()
    {
        UIManager.Get().SetPauseMenuUI(true);
        GameManager.Instance.SetPause(true);
    }
    private void ResumingGameplay()
    {
        UIManager.Get().SetPauseMenuUI(false);
        GameManager.Instance.SetPause(false);
        SendEvent(Events.ResumeGameplay);
    }
}
