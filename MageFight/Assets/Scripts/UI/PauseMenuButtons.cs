using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons :MonoBehaviour
{
    public void OnRematchButtonPressed()
    {
        GameManager.Instance.SetPause(false);
        AkSoundEngine.PostEvent(AudioEvents.eventsIDs[AudioEvents.EventsKeys.Rematch_Pressed.ToString()], this.gameObject);
        GameplayManager.Get().SendEvent(GameplayManager.Events.RematchSelected);
    }
    public void OnMainMenuButtonPressed()
    {
        GameManager.Instance.SetPause(false);
        //AkSoundEngine.PostEvent(AudioEvents.eventsIDs[AudioEvents.EventsKeys.MainMenu_Pressed.ToString()], this.gameObject);
        LoaderManager.Get().LoadScene("MainMenu");
    }
    public void OnResumeButtonPressed()
    {
        AkSoundEngine.PostEvent(AudioEvents.eventsIDs[AudioEvents.EventsKeys.Resume_Pressed.ToString()], this.gameObject);
        GameplayManager.Get().SendEvent(GameplayManager.Events.PauseGameplay);
    }
}