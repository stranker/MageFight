using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons : MonoBehaviour
{
    public void OnRematchButtonPressed()
    {
        GameManager.Instance.SetPause(false);
        GameplayManager.Get().SendEvent(GameplayManager.Events.RematchSelected);
    }
    public void OnExitButtonPressed()
    {
        GameManager.Instance.SetPause(false);
        LoaderManager.Get().LoadScene("MainMenu");
    }
    public void OnResumeButtonPressed()
    {
        GameplayManager.Get().SendEvent(GameplayManager.Events.PauseGameplay);
    }
}