using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginMatch : MonoBehaviour
{

    public Animator anim;
    [SerializeField] private Text roundText;

    public void BeginCountdown()
    {
        anim.SetTrigger("BeginRound");
        roundText.text = "ROUND " + (GameManager.Instance.GetCurrentRound() + 1).ToString();
    }

    public void EndCountDown()
    {
        GameplayManager.Get().SendEvent(GameplayManager.Events.CountdownEnd);
        gameObject.SetActive(false);
    }

}
