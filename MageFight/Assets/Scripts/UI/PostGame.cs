using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PostGame : MonoBehaviour
{

    [SerializeField] private Text winnerLabel;
    [SerializeField] private Animator anim;

    public void SetWinner(int winnerIdx)
    {
        gameObject.SetActive(true);
        winnerLabel.text = "PLAYER " + winnerIdx.ToString();
        anim.SetTrigger("Enter");

    }

    public void Hide()
    {
        anim.SetTrigger("Leave");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
