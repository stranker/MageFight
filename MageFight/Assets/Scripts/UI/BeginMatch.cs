using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginMatch : MonoBehaviour
{

    public Animator anim;
    public Text countdown;

    public void BeginCountdown()
    {
        anim.SetTrigger("BeginRound");
    }

    public void EndCountDown()
    {
        GameplayManager.Get().SendEvent(GameplayManager.Events.CountdownEnd);
        gameObject.SetActive(false);
        countdown.text = "3";
    }

    public void ChangeTextTo(string text)
    {
        countdown.text = text;
    }

}
